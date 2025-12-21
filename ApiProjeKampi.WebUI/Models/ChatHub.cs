using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Net;
using Microsoft.Extensions.Logging;

namespace ApiProjeKampi.WebUI.Models
{
    public class ChatHub : Hub
    {
        // Gemini API Key buraya gelecek
        private const string apiKey = "";
        // ListModels sonucuna göre modelName güncellendi
        private const string modelName = "gemini-2.5-flash";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IHttpClientFactory httpClientFactory, ILogger<ChatHub> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
                
        // Gemini formatına uygun geçmiş yönetimi (role: user/model)
        private static readonly Dictionary<string, List<GeminiContent>> _history = new();

        public override Task OnConnectedAsync()
        {
            // Gemini'de sistem talimatı genelde ayrı bir parametre veya ilk mesaj olarak gönderilir.
            // Burada geçmişi boş başlatıyoruz.
            _history[Context.ConnectionId] = new List<GeminiContent>();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _history.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string userMessage)
        {
            await Clients.Caller.SendAsync("ReceiveUserEcho", userMessage);

            var history = _history[Context.ConnectionId];
            history.Add(new GeminiContent
            {
                Role = "user",
                Parts = new List<GeminiPart> { new GeminiPart { Text = userMessage } }
            });

            await StreamGeminiAI(history, Context.ConnectionAborted);
        }

        public async Task StreamGeminiAI(List<GeminiContent> history, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("gemini");

            // Denenecek endpointler (öncelik v1, sonra v1beta)
            var endpoints = new[]
            {
                $"v1/models/{modelName}:streamGenerateContent?key={apiKey}",
                $"v1beta/models/{modelName}:streamGenerateContent?key={apiKey}"
            };

            var payload = new { contents = history };

            try
            {
                HttpResponseMessage resp = null;
                string lastErrorPreview = null;

                // Endpoint fallback
                foreach (var relativeUrl in endpoints)
                {
                    using var req = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
                    req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                    _logger.LogDebug("Trying Gemini endpoint {Endpoint}", relativeUrl);
                    resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (resp.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Using endpoint {Endpoint}", relativeUrl);
                        break;
                    }

                    var errBody = await resp.Content.ReadAsStringAsync(cancellationToken);
                    lastErrorPreview = string.IsNullOrEmpty(errBody) ? "(empty body)" : (errBody.Length > 1500 ? errBody.Substring(0, 1500) + "..." : errBody);
                    _logger.LogWarning("Endpoint {Endpoint} returned {Status}. Preview: {Preview}", relativeUrl, resp.StatusCode, lastErrorPreview);
                }

                if (resp == null)
                {
                    await Clients.Caller.SendAsync("ReceiveToken", "Hata: İstek oluşturulamadı.");
                    await Clients.Caller.SendAsync("CompleteMessage", string.Empty);
                    return;
                }

                if (!resp.IsSuccessStatusCode)
                {
                    var status = resp.StatusCode;
                    try
                    {
                        var listResp = await client.GetAsync($"v1/models?key={apiKey}", cancellationToken);
                        var listBody = await listResp.Content.ReadAsStringAsync(cancellationToken);
                        var listPreview = string.IsNullOrEmpty(listBody) ? "(no list body)" : (listBody.Length > 2000 ? listBody.Substring(0, 2000) + "..." : listBody);

                        await Clients.Caller.SendAsync("ReceiveToken", $"Hata: {status} - {lastErrorPreview}\nAvailable models (preview):\n{listPreview}");
                    }
                    catch (Exception exList)
                    {
                        await Clients.Caller.SendAsync("ReceiveToken", $"Hata: {status} - {lastErrorPreview}\nListModels error: {exList.Message}");
                    }

                    await Clients.Caller.SendAsync("CompleteMessage", string.Empty);
                    return;
                }

                // Eğer içerik Length header'ı varsa ve küçükse non-stream olabilir; burayı debug için göster
                if (resp.Content.Headers.ContentLength.HasValue && resp.Content.Headers.ContentLength.Value < 1024)
                {
                    var body = await resp.Content.ReadAsStringAsync(cancellationToken);
                    var preview = string.IsNullOrEmpty(body) ? "(empty body)" : (body.Length > 8000 ? body.Substring(0, 8000) + "..." : body);
                    _logger.LogDebug("Gemini returned short body (len {Len}) - preview sent to client", body.Length);
                    await Clients.Caller.SendAsync("ReceiveToken", $"[DBG_FULLBODY] {preview}");
                    await Clients.Caller.SendAsync("CompleteMessage", string.Empty);
                    return;
                }

                // Stream okuma (daha dayanıklı: kısmi JSON biriktir ve tam JSON objelerini ayıkla)
                using var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
                using var reader = new StreamReader(stream);
                var sb = new StringBuilder();
                var partial = new StringBuilder();
                bool anyTokenSent = false;

                string ExtractNextJsonObject(StringBuilder builder)
                {
                    var s = builder.ToString();
                    int start = s.IndexOf('{');
                    if (start < 0) return null;
                    int depth = 0;
                    for (int i = start; i < s.Length; i++)
                    {
                        if (s[i] == '{') depth++;
                        else if (s[i] == '}')
                        {
                            depth--;
                            if (depth == 0)
                            {
                                int len = i - start + 1;
                                var obj = s.Substring(start, len);
                                // remove up to i+1
                                builder.Remove(0, i + 1);
                                return obj;
                            }
                        }
                    }
                    return null;
                }

                while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string cleanLine = line.Trim();
                    if (cleanLine == "[" || cleanLine == "]") continue;
                    if (cleanLine.StartsWith(",")) cleanLine = cleanLine.Substring(1).Trim();

                    _logger.LogDebug("Received stream chunk (len {Len})", cleanLine.Length);
                    // Append chunk to partial buffer
                    partial.Append(cleanLine);

                    // Extract all complete JSON objects from the buffer
                    while (true)
                    {
                        var objText = ExtractNextJsonObject(partial);
                        if (objText == null) break;

                        try
                        {
                            using var doc = JsonDocument.Parse(objText);
                            if (doc.RootElement.TryGetProperty("candidates", out var candidates) &&
                                candidates.GetArrayLength() > 0 &&
                                candidates[0].TryGetProperty("content", out var content) &&
                                content.TryGetProperty("parts", out var parts) &&
                                parts.GetArrayLength() > 0)
                            {
                                var text = parts[0].GetProperty("text").GetString();
                                if (!string.IsNullOrEmpty(text))
                                {
                                    sb.Append(text);
                                    anyTokenSent = true;
                                    await Clients.Caller.SendAsync("ReceiveToken", text);
                                }
                            }
                            else
                            {
                                // Beklenen yapıda değilse debug olarak logla (istemciye gönderme zorunlu değil)
                                _logger.LogDebug("Parsed JSON object did not contain expected fields.");
                            }
                        }
                        catch (JsonException jex)
                        {
                            // Eğer parse hatası olursa, logla ve devam et (parça bozuk olabilir)
                            _logger.LogDebug(jex, "Failed to parse extracted JSON object.");
                        }
                    }
                }

                var fullResponse = sb.ToString();
                if (anyTokenSent && !string.IsNullOrEmpty(fullResponse))
                {
                    history.Add(new GeminiContent
                    {
                        Role = "model",
                        Parts = new List<GeminiPart> { new GeminiPart { Text = fullResponse } }
                    });
                    await Clients.Caller.SendAsync("CompleteMessage", fullResponse);
                }
                else
                {
                    _logger.LogInformation("Stream completed but no model tokens were emitted.");
                    await Clients.Caller.SendAsync("ReceiveToken", "[DBG] Stream tamamlandı fakat token parse edilmedi.");
                    await Clients.Caller.SendAsync("CompleteMessage", string.Empty);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in StreamGeminiAI");
                await Clients.Caller.SendAsync("ReceiveToken", $"Sistem Hatası: {ex.Message}");
                await Clients.Caller.SendAsync("CompleteMessage", string.Empty);
            }
        }

        #region Gemini Modelleri
        public class GeminiContent
        {                   
            [JsonPropertyName("role")] public string Role { get; set; }
            [JsonPropertyName("parts")] public List<GeminiPart> Parts { get; set; }
        }

        public class GeminiPart
        {
            [JsonPropertyName("text")] public string Text { get; set; }
        }

        public class GeminiResponse
        {
            [JsonPropertyName("candidates")] public List<Candidate> Candidates { get; set; }
        }

        public class Candidate
        {
            [JsonPropertyName("content")] public GeminiContent Content { get; set; }
        }
        #endregion
    }
}