using APIProjeKampi_WepApi.Context;
using APIProjeKampi_WepApi.Dtos.GroupReservationDtos;
using APIProjeKampi_WepApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProjeKampi_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupReservationsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public GroupReservationsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GroupReservationList()
        {
            var values = _context.GroupReservations.ToList();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateGroupReservation(CreatGroupReservationDto creaateGroupReservationDto)
        {
            //_context.GroupReservations.Add(GroupReservation);
            //_context.SaveChanges();
            var value = _mapper.Map<GroupReservation>(creaateGroupReservationDto);
            _context.GroupReservations.Add(value);
            _context.SaveChanges();
            return Ok("Ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteGroupReservation(int id)
        {
            var value = _context.GroupReservations.Find(id);
            _context.GroupReservations.Remove(value);
            _context.SaveChanges();
            return Ok("Silme işlemi başarılı");
        }
        [HttpGet("GetGroupReservation")]
        public IActionResult GetGroupReservation(int id)
        {
            var values = _context.GroupReservations.Find(id);
            return Ok(values);
        }
        [HttpPut]
        public IActionResult UpdateGroupReservations(UpdateGroupReservationDto updateGroupReservationDto)
        {

            var value = _mapper.Map<GroupReservation>(updateGroupReservationDto);
            _context.GroupReservations.Update(value);
            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarılı");

        }
    }
}
