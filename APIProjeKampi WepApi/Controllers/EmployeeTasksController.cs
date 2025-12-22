using APIProjeKampi_WepApi.Context;
using APIProjeKampi_WepApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProjeKampi_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTasksController : ControllerBase
    {
        private readonly ApiContext _context;

        public EmployeeTasksController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult TestimonialList()
        {
            var values = _context.EmployeeTasks.ToList();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateTestimonial(EmployeeTask EmployeeTask)
        {
            _context.EmployeeTasks.Add(EmployeeTask);
            _context.SaveChanges();
            return Ok("Ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteTestimonial(int id)
        {
            var value = _context.EmployeeTasks.Find(id);
            _context.EmployeeTasks.Remove(value);
            _context.SaveChanges();
            return Ok("Silme işlemi başarılı");
        }
        [HttpGet("GetTestimonial")]
        public IActionResult GetTestimonial(int id)
        {
            var values = _context.EmployeeTasks.Find(id);
            return Ok(values);
        }
        [HttpPut]
        public IActionResult UpdateTestimonials(EmployeeTask Testimonial)
        {
            _context.EmployeeTasks.Update(Testimonial);
            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarılı");

        }
    }
}
