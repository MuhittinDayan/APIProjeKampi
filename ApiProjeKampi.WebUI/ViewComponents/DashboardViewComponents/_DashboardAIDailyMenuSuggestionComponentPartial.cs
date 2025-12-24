using ApiProjeKampi.WebUI.Dtos.AISuggestionDtos;
using ApiProjeKampi.WebUI.Dtos.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjeKampi.WebUI.ViewComponents.DashboardViewComponents
{
    public class _DashboardAIDailyMenuSuggestionComponentPartial : ViewComponent
    {
        // BURAYA GEMINI API KEY'INIZI YAZIN
        private const string apiKey = "";

        private readonly IHttpClientFactory _httpClientFactory;

        // 150+ Ülke Mutfağı Listesi (Static tanımladık ki her istekte bellekte yeniden oluşturulmasın)
        private static readonly string[] CountryList = new[]
        {
            "Afganistan", "Arnavutluk", "Cezayir", "Andorra", "Angola", "Arjantin", "Ermenistan", "Avustralya", "Avusturya", "Azerbaycan",
            "Bahamalar", "Bahreyn", "Bangladeş", "Barbados", "Beyaz Rusya", "Belçika", "Belize", "Benin", "Butan", "Bolivya",
            "Bosna Hersek", "Botsvana", "Brezilya", "Brunei", "Bulgaristan", "Burkina Faso", "Burundi", "Kamboçya", "Kamerun", "Kanada",
            "Yeşil Burun Adaları", "Orta Afrika Cumhuriyeti", "Çad", "Şili", "Çin", "Kolombiya", "Komorlar", "Kongo", "Kosta Rika", "Hırvatistan",
            "Küba", "Kıbrıs", "Çekya", "Danimarka", "Cibuti", "Dominik Cumhuriyeti", "Ekvador", "Mısır", "El Salvador", "Ekvator Ginesi",
            "Eritre", "Estonya", "Etiyopya", "Fiji", "Finlandiya", "Fransa", "Gabon", "Gambiya", "Gürcistan", "Almanya",
            "Gana", "Yunanistan", "Grenada", "Guatemala", "Gine", "Guyana", "Haiti", "Honduras", "Macaristan", "İzlanda",
            "Hindistan", "Endonezya", "İran", "Irak", "İrlanda", "İsrail", "İtalya", "Fildişi Sahili", "Jamaika", "Japonya",
            "Ürdün", "Kazakistan", "Kenya", "Kuveyt", "Kırgızistan", "Laos", "Letonya", "Lübnan", "Lesotho", "Liberya",
            "Libya", "Lihtenştayn", "Litvanya", "Lüksemburg", "Madagaskar", "Malavi", "Malezya", "Maldivler", "Mali", "Malta",
            "Moritanya", "Mauritius", "Meksika", "Moldova", "Monako", "Moğolistan", "Karadağ", "Fas", "Mozambik", "Myanmar",
            "Namibya", "Nepal", "Hollanda", "Yeni Zelanda", "Nikaragua", "Nijer", "Nijerya", "Kuzey Makedonya", "Norveç", "Umman",
            "Pakistan", "Filistin", "Panama", "Papua Yeni Gine", "Paraguay", "Peru", "Filipinler", "Polonya", "Portekiz", "Katar",
            "Romanya", "Rusya", "Ruanda", "Saint Lucia", "Samoa", "San Marino", "Suudi Arabistan", "Senegal", "Sırbistan", "Seyşeller",
            "Sierra Leone", "Singapur", "Slovakya", "Slovenya", "Somali", "Güney Afrika", "Güney Kore", "İspanya", "Sri Lanka", "Sudan",
            "Surinam", "İsveç", "İsviçre", "Suriye", "Tayvan", "Tacikistan", "Tanzanya", "Tayland", "Togo", "Tonga",
            "Tunus", "Türkiye", "Türkmenistan", "Uganda", "Ukrayna", "Birleşik Arap Emirlikleri", "Birleşik Krallık", "Amerika Birleşik Devletleri", "Uruguay", "Özbekistan",
            "Vanuatu", "Venezuela", "Vietnam", "Yemen", "Zambiya", "Zimbabve"
        };
        public _DashboardAIDailyMenuSuggestionComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // İstemciyi oluştur
            var client = _httpClientFactory.CreateClient();

            // 1. ADIM: Bu dev listeden rastgele 4 tane seçiyoruz
            var random = new Random();
            var selectedCuisines = CountryList.OrderBy(x => random.Next()).Take(4).ToList();

            string selectedCuisinesString = string.Join(", ", selectedCuisines);

            // Prompt Metni (Aynı kalabilir, biraz optimize ettim)
            string prompt = $@"
                            Aşağıda listelediğim 4 ülke mutfağı için bana birer tane, o ülkeye özgü 4 çeşitten oluşan dengeli bir yemek menüsü oluştur.

                            SEÇİLEN ÜLKELER: {selectedCuisinesString}

                            KURALLAR:
                            - Listede verdiğim ülkelerin dışına çıkma.
                            - Ülke adını ve ISO kodunu doğru eşleştir.
                            - Yemek isimleri orijinal, açıklamaları Türkçe olsun.
                            - Fiyatlar tahmini olsun (TL cinsinden).
                            - Her menü (Items listesi) SADECE ve SIRASIYLA şu 4 kategoriden oluşmalıdır:
                                 1. Çorba
                                 2. Ara Sıcak veya Başlangıç
                                 3. Ana Yemek
                                 4. Tatlı
                            - Cevap SADECE geçerli bir JSON dizisi olsun. Markdown (```json) kullanma.

                            JSON FORMATI:
                            [
                              {{ 
                                ""Cuisine"": ""Ülke Adı Mutfağı"", 
                                ""CountryCode"": ""XX"", 
                                ""MenuTitle"": ""Günlük Menü"", 
                                ""Items"": [ 
                                   {{ ""Name"": ""[Çorba Adı]"", ""Description"": ""..."", ""Price"": 90 }},
                                   {{ ""Name"": ""[Ara Sıcak Adı]"", ""Description"": ""..."", ""Price"": 120 }},
                                   {{ ""Name"": ""[Ana Yemek Adı]"", ""Description"": ""..."", ""Price"": 250 }},
                                   {{ ""Name"": ""[Tatlı Adı]"", ""Description"": ""..."", ""Price"": 100 }}
                                ] 
                              }}
                            ]
                            ";

            // Gemini için İstek Gövdesi (Request Body)
            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    response_mime_type = "application/json" // Gemini'yi JSON moduna zorlar
                }
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Gemini API Endpoint'i (Flash modelini kullanıyoruz, hızlı ve verimli)
            var response = await client.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}", content);

            List<MenuSuggestionDto> menus = new();

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                dynamic obj = JsonConvert.DeserializeObject(responseJson);

                try
                {
                    string aiContent = obj.candidates[0].content.parts[0].text.ToString();

                    // Olası markdown temizliği
                    aiContent = aiContent.Replace("```json", "").Replace("```", "").Trim();

                    menus = JsonConvert.DeserializeObject<List<MenuSuggestionDto>>(aiContent);
                }
                catch
                {
                    menus = new();
                }
            }

            return View(menus);
        }
    }
}