using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Markdig; // Markdown metinlerini HTML'e dönüştürmek için kullanılan kütüphane

namespace ApiProjeKampi.WebUI.Controllers
{
    public class AIController : Controller
    {
        // Sayfa ilk kez yüklendiğinde çalışan metod (GET)
        public IActionResult CreateRecipeWithGeminiAI()
        {
            return View();
        }

        // Kullanıcı "Tarif Oluştur" butonuna bastığında çalışan metod (POST)
        [HttpPost]
        public async Task<IActionResult> CreateRecipeWithGeminiAI(string prompt)
        {
            // 1. KONTROL: Kullanıcı hiçbir şey yazmadan butona bastıysa uyar
            if (string.IsNullOrEmpty(prompt))
            {
                ViewBag.recipe = "Lütfen malzeme listesi giriniz.";
                return View();
            }

            // 2. YETKİLENDİRME: Google AI Studio'dan alınan benzersiz anahtar
            var apiKey = "";

            // 3. HTTP İSTEMCİSİ: Google sunucularına istek atmak için kullanılan nesne
            using var client = new HttpClient();

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
                                text = $"Sen dünya çapında ödüllü bir şefsin. " +
                                       $"Verilen malzemelerle harika, detaylı ve iştah açıcı bir yemek tarifi üret." +
                                       $" Başlıklar, maddeler ve profesyonel ipuçları ekle. Malzemeler: {prompt}"
                            }
                        }
                    }
                }
            };

            // 6. İSTEĞİ GÖNDER: Hazırlanan veriyi JSON olarak Google'a gönder ve cevabı bekle (await)
            var response = await client.PostAsJsonAsync(url, requestData);

            // 7. CEVABI İŞLE: Eğer Google "Tamam" (200 OK) dediyse
            if (response.IsSuccessStatusCode)
            {
                // Gelen karmaşık JSON cevabını, aşağıda tanımladığımız sınıflara (Model) dönüştür
                var result = await response.Content.ReadFromJsonAsync<GeminiResponseModel>();

                // Cevap hiyerarşisi içinden asıl metni (tarifi) çekip çıkar
                var content = result?.candidates?[0]?.content?.parts?[0]?.text;

                if (!string.IsNullOrEmpty(content))
                {
                    // 8. MARKDOWN DÖNÜŞTÜRME: Gemini metni yıldızlarla (**) gönderir. 
                    // Markdig kütüphanesi bu yıldızları <b> veya <li> gibi HTML etiketlerine çevirir.
                    var pipeline = new MarkdownPipelineBuilder().Build();
                    var htmlContent = Markdown.ToHtml(content, pipeline);

                    // Temizlenmiş HTML içeriğini ekranda göstermek üzere ViewBag'e at
                    ViewBag.recipe = htmlContent;
                }
            }
            else
            {
                // Eğer hata varsa (Hatalı API Key, internet sorunu vb.) kullanıcıya hata kodunu göster
                ViewBag.recipe = "Bir hata oluştu: " + response.StatusCode;
            }

            return View();
        }

        /* --- GOOGLE GEMINI CEVAP YAPISINI KARŞILAYAN SINIFLAR (DTOs) --- */
        // Bu sınıflar, Google'dan gelen karışık JSON verisini C# nesnelerine eşlemek için kullanılır.

        public class GeminiResponseModel
        {
            public List<Candidate> candidates { get; set; } // AI'nın ürettiği muhtemel cevaplar listesi
        }

        public class Candidate
        {
            public Content content { get; set; } // Seçilen cevabın içeriği
        }

        public class Content
        {
            public List<Part> parts { get; set; } // Cevabın parçaları (Metin burada saklanır)
        }

        public class Part
        {
            public string text { get; set; } // Asıl tarif metni
        }
    }
}