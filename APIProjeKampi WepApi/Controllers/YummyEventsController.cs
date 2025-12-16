using APIProjeKampi_WepApi.Context;
using APIProjeKampi_WepApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProjeKampi_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YummyEventsController : ControllerBase
    {
        private readonly ApiContext _context;

        public YummyEventsController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult YummyEventList()
        {
            var values = _context.YummyEvents.ToList();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateYummyEvent(YummyEvent YummyEvent)
        {
            _context.YummyEvents.Add(YummyEvent);
            _context.SaveChanges();
            return Ok("Etkinlik ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteYummyEvent(int id)
        {
            var value = _context.YummyEvents.Find(id);
            _context.YummyEvents.Remove(value);
            _context.SaveChanges();
            return Ok("Etkinlik silme işlemi başarılı");
        }
        [HttpGet("GetYummyEvent")]
        public IActionResult GetYummyEvent(int id)
        {
            var values = _context.YummyEvents.Find(id);
            return Ok(values);
        }
        [HttpPut]
        public IActionResult UpdateYummyEvents(YummyEvent YummyEvent)
        {
            _context.YummyEvents.Update(YummyEvent);
            _context.SaveChanges();
            return Ok("Etkinlik güncelleme işlemi başarılı");
        }
    }
}
