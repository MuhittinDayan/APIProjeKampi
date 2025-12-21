using ApiProjeKampi.WebUI.Dtos.CategoryDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace ApiProjeKampi.WebUI.ViewComponents.DashboardViewComponents
{
    public class _DashboardWidgetsComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _DashboardWidgetsComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int r1, r2, r3, r4;
            Random rnd = new Random();
            r1 = rnd.Next(1,35);
            r2 = rnd.Next(1,35);
            r3 = rnd.Next(1,35);
            r4 = rnd.Next(1,35);



            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7189/api/Reservations/GetTotalReservationCount");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            ViewBag.v1 = jsonData;
            ViewBag.r1 = r1;


            var client_2 = _httpClientFactory.CreateClient();
            var responseMessage_2 = await client_2.GetAsync("https://localhost:7189/api/Reservations/GetTotalCustomerCount\r\n");
            var jsonData_2 = await responseMessage_2.Content.ReadAsStringAsync();
            ViewBag.v2 = jsonData_2;
            ViewBag.r2 = r2;


            var client_3 = _httpClientFactory.CreateClient();
            var responseMessage_3 = await client_3.GetAsync("https://localhost:7189/api/Reservations/GetPendingReservations\r\n");
            var jsonData_3 = await responseMessage_3.Content.ReadAsStringAsync();
            ViewBag.v3 = jsonData_3;
            ViewBag.r3 = r3;




            var client_4 = _httpClientFactory.CreateClient();
            var responseMessage_4 = await client_4.GetAsync("https://localhost:7189/api/Reservations/GetApprovedReservations\r\n");
            var jsonData_4 = await responseMessage_4.Content.ReadAsStringAsync();
            ViewBag.v4 = jsonData_4;
            ViewBag.r4 = r4;



            return View();
        }
    }
}
