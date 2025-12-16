using APIProjeKampi_WepApi.Context;
using APIProjeKampi_WepApi.Dtos.AboutDtos;
using APIProjeKampi_WepApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIProjeKampi_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;

        public AboutsController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public IActionResult AboutList()
        {
            var values = _context.Abouts.ToList();
            return Ok(_mapper.Map<List<ResultAboutDto>>(values));
        }
        [HttpPost]
        public IActionResult CreateAbout(CreateAboutDto createAboutDto)
        {
            var value = _mapper.Map<About>(createAboutDto);
            _context.Abouts.Add(value);
            _context.SaveChanges();
            return Ok("Hakkımda alanı ekleme İşlemi Başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            _context.Abouts.Remove(value);
            _context.SaveChanges();
            return Ok("Hakkımda alanı silme İşlemi Başarılı");
        }

        [HttpGet("GetById")]
        public IActionResult GetAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            return Ok(_mapper.Map<GetAboutByIdDto>(value));
        }

        [HttpPut]
        public IActionResult UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            var value = _mapper.Map<About>(updateAboutDto);
            _context.Abouts.Update(value);
            _context.SaveChanges();
            return Ok("Hakkımda alanı güncelleme işlemi başarılı");
        }
    }
}
