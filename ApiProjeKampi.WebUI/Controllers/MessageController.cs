using ApiProjeKampi.WebUI.Dtos.MessageDtos;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static ApiProjeKampi.WebUI.Controllers.AIController;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public MessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> MessageList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7189/api/Messages");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7189/api/Messages", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MessageList");
            }
            return View();
        }
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync("https://localhost:7189/api/Messages?id=" + id);
            return RedirectToAction("MessageList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMessage(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7189/api/Messages/GetMessage?id=" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetMessageByIdDto>(jsonData);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            await client.PutAsync("https://localhost:7189/api/Messages/", stringContent);
            return RedirectToAction("MessageList");
        }

        [HttpGet]
        public async Task<IActionResult> AnswerMessageWithGeminiAI(int id ,string prompt)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7189/api/Messages/GetMessage?id=" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetMessageByIdDto>(jsonData);
            var customerPrompt = value.MessageDetails;

            // 2.YETKİLENDİRME: Google AI Studio'dan alınan benzersiz anahtar
            var apiKey = "";

            // 3. HTTP İSTEMCİSİ: Google sunucularına istek atmak için kullanılan nesne
            using var client_2 = new HttpClient();

            // 4. ADRES (URL): Gemini 2.0 Flash modeline ulaşacağımız uç nokta (endpoint)
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            // 5. VERİ PAKETİ (REQUEST): Google'ın beklediği özel JSON formatında isteği hazırlıyoruz
            var requestData = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { 
                                // Yapay zekaya giden ana talimat (Sistem Mesajı + Kullanıcı Girdisi)
                                text = $@"Sen bir restoranın müşteri ilişkileri temsilcisisin. 
                                        Aşağıda sana {value.NameSurname} olan müşteriden gelen mesaj iletildi. 

                                        GÖREVİN:
                                        1. Bu mesaja nazik ve profesyonel bir yanıt yaz.
                                        2. ÖNEMLİ: Müşterinin mesajında belirtmediği yemek isimleri, hizmetler veya detaylar hakkında uydurma yorum yapma. Sadece müşterinin bahsettiği konulara değin.
                                        3. Eğer müşteri genel bir memnuniyet belirttiyse, sen de genel bir teşekkür et.
                                        4. Müşteriye ismiyle hitap et (Müşteri: {value.NameSurname}).
                                        5. Sadece cevap metnini döndür.

                                        MÜŞTERİ MESAJI:
                                        {customerPrompt}"
        }
                        }
                    }
                }
            };

            // 6. İSTEĞİ GÖNDER: Hazırlanan veriyi JSON olarak Google'a gönder ve cevabı bekle (await)
            var response = await client_2.PostAsJsonAsync(url, requestData);

            // 7. CEVABI İŞLE: Eğer Google "Tamam" (200 OK) dediyse
            if (response.IsSuccessStatusCode)
            {
                // Gelen karmaşık JSON cevabını, aşağıda tanımladığımız sınıflara (Model) dönüştür
                var result = await response.Content.ReadFromJsonAsync<GeminiResponseModel>();

                // Cevap hiyerarşisi içinden asıl metni (tarifi) çekip çıkar
                var aiResponseText = result?.candidates?[0]?.content?.parts?[0]?.text;

                if (!string.IsNullOrEmpty(aiResponseText))
                {
                    var pipeline = new MarkdownPipelineBuilder().Build();
                    var htmlContent = Markdown.ToHtml(aiResponseText, pipeline);

                    // AI cevabını ViewBag'e koymak yerine doğrudan modelin içine yazalım (daha güvenli)
                    ViewBag.answerAI = htmlContent; // HTML formatı için saklamaya devam edebilirsin
                    ViewBag.rawAiAnswer = aiResponseText;
                }
            }
            else
            {
                // Eğer hata varsa (Hatalı API Key, internet sorunu vb.) kullanıcıya hata kodunu göster
                ViewBag.answerAI = "Bir hata oluştu: " + response.StatusCode;
            }
            return View(value);
        }

        public PartialViewResult SendMessage()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task< IActionResult> SendMessage(CreateMessageDto createMessageDto)
        {
            var client= new HttpClient();
            var token = "";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            try
            {
                var translateRequestBody = new
                {
                    inputs = createMessageDto.MessageDetails
                };
                var translateJson = JsonSerializer.Serialize(translateRequestBody);
                var translateContent = new StringContent(translateJson,Encoding.UTF8,"application/json");

                var translateResponse = await client.PostAsync("https://router.huggingface.co/hf-inference/models/Helsinki-NLP/opus-mt-tr-en", translateContent);
                var translateResponseString = await translateResponse.Content.ReadAsStringAsync();

                string englishText = createMessageDto.MessageDetails;
                if (translateResponseString.TrimStart().StartsWith("["))
                {
                    var translateDoc = JsonDocument.Parse(translateResponseString);
                    englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString();
                    //ViewBag.v = englishText;

                }

                var toxicRequestBody = new
                {
                    inputs = englishText
                };

                var toxicJson = JsonSerializer.Serialize(toxicRequestBody);
                var toxicContent = new StringContent(toxicJson,Encoding.UTF8,"application/json");
                var toxicResponse = await client.PostAsync("https://router.huggingface.co/hf-inference/models/unitary/toxic-bert", toxicContent);
                var toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();

                if(toxicResponseString.TrimStart().StartsWith("["))
                {
                    var toxicDoc = JsonDocument.Parse(toxicResponseString);
                    foreach(var item in toxicDoc.RootElement[0].EnumerateArray())
                    {
                        string label = item.GetProperty("label").GetString();
                        double score = item.GetProperty("score").GetDouble();

                        if(score>0.5)
                        {
                            createMessageDto.Status = "Toksik Mesaj";
                            break;
                        }
                    }
                }

                if(string.IsNullOrEmpty(createMessageDto.Status))
                {
                    createMessageDto.Status = "Mesaj Alındı";
                }

            }
            catch(Exception ex)
            {
                createMessageDto.Status = "Onay Bekliyor";
            }
            




            var client_2 = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client_2.PostAsync("https://localhost:7189/api/Messages", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MessageList");
            }
            return View();
        }
    }
}
