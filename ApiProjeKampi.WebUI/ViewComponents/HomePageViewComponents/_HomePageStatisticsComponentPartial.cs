using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebUI.ViewComponents.HomePageViewComponents
{
    public class _HomePageStatisticsComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _HomePageStatisticsComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task< IViewComponentResult> InvokeAsync()
        {
            // ProductCount
            var client_1 = _httpClientFactory.CreateClient();
            var responseMessage_1 = await client_1.GetAsync("https://localhost:7189/api/Statistics/ProductCount");
            var jsonData_1 = await responseMessage_1.Content.ReadAsStringAsync();
            ViewBag.v1 = jsonData_1;

            // ReservationCount
            var client_2 = _httpClientFactory.CreateClient();
            var responseMessage_2 = await client_2.GetAsync("https://localhost:7189/api/Statistics/ReservationCount");
            var jsonData_2 = await responseMessage_2.Content.ReadAsStringAsync();
            ViewBag.v2 = jsonData_2;

            // ChefCount
            var client_3 = _httpClientFactory.CreateClient();
            var responseMessage_3 = await client_3.GetAsync("https://localhost:7189/api/Statistics/ChefCount");
            var jsonData_3 = await responseMessage_3.Content.ReadAsStringAsync();
            ViewBag.v3 = jsonData_3;

            // TotalGuestCount
            var client_4 = _httpClientFactory.CreateClient();
            var responseMessage_4 = await client_4.GetAsync("https://localhost:7189/api/Statistics/TotalGuestCount");
            var jsonData_4 = await responseMessage_4.Content.ReadAsStringAsync();
            ViewBag.v4 = jsonData_4;

            return View();
        }
    }
}
