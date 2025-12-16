using APIProjeKampi_WepApi.Context;
using APIProjeKampi_WepApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProjeKampi_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialsController : ControllerBase
    {
        private readonly ApiContext _context;

        public TestimonialsController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult TestimonialList()
        {
            var values = _context.Testiomanials.ToList();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateTestimonial(Testiomanial Testiomanial)
        {
            _context.Testiomanials.Add(Testiomanial);
            _context.SaveChanges();
            return Ok("Referans ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteTestimonial(int id)
        {
            var value = _context.Testiomanials.Find(id);
            _context.Testiomanials.Remove(value);
            _context.SaveChanges();
            return Ok("Referans silme işlemi başarılı");
        }
        [HttpGet("GetTestimonial")]
        public IActionResult GetTestimonial(int id)
        {
            var values = _context.Testiomanials.Find(id);
            return Ok(values);
        }
        [HttpPut]
        public IActionResult UpdateTestimonials(Testiomanial Testimonial)
        {
            _context.Testiomanials.Update(Testimonial);
            _context.SaveChanges();
            return Ok("Referans güncelleme işlemi başarılı");

        }
    }
}
