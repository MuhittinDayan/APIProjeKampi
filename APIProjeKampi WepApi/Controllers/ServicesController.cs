using APIProjeKampi_WepApi.Context;
using APIProjeKampi_WepApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProjeKampi_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ApiContext _context;

        public ServicesController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult ServiceList()
        {
            var values = _context.Services.ToList();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult Createservice(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
            return Ok("Hizmet ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteService(int id)
        {
            var value = _context.Services.Find(id);
            _context.Services.Remove(value);
            _context.SaveChanges();
            return Ok("Hizmet silme işlemi başarılı");
        }
        [HttpGet("GetService")]
        public IActionResult GetService(int id)
        {
            var values = _context.Services.Find(id);
            return Ok(values);
        }
        [HttpPut]
        public IActionResult UpdateServices(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
            return Ok("Hizmet güncelleme işlemi başarılı");

        }

    }
}
