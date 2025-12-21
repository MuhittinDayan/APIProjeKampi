using APIProjeKampi_WepApi.Context;
using APIProjeKampi_WepApi.Dtos.ReservationDtos;
using APIProjeKampi_WepApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProjeKampi_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ReservationsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult ReservationList()
        {
            var values = _context.Reservations.ToList();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateReservation(CreateReservationDto creaateReservationDto)
        {
            var value = _mapper.Map<Reservation>(creaateReservationDto);
            _context.Reservations.Add(value);
            _context.SaveChanges();
            return Ok("Rezervasyon ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteReservation(int id)
        {
            var value = _context.Reservations.Find(id);
            _context.Reservations.Remove(value);
            _context.SaveChanges();
            return Ok("Rezervasyon silme işlemi başarılı");
        }
        [HttpGet("GetReservation")]
        public IActionResult GetReservation(int id)
        {
            var values = _context.Reservations.Find(id);
            return Ok(values);
        }
        [HttpPut]
        public IActionResult UpdateReservation(UpdateReservationDto updateReservationDto)
        {

            var value = _mapper.Map<Reservation>(updateReservationDto);
            _context.Reservations.Update(value);
            _context.SaveChanges();
            return Ok("Rezervasyon güncelleme işlemi başarılı");

        }

        [HttpGet("GetTotalReservationCount")]
        public IActionResult GetTotalReservationCount()
        {
            var value = _context.Reservations.Count();
            return Ok(value);
        }

        [HttpGet("GetTotalCustomerCount")]
        public IActionResult GetTotalCustomerCount()
        {
            var value = _context.Reservations.Sum(x => x.CountOfPeople);
            return Ok(value);
        }

        [HttpGet("GetPendingReservations")]
        public IActionResult GetPendingReservations()
        {
            var value = _context.Reservations.Where(x => x.ReservationStatus == "Onay Bekliyor").Count();
            return Ok(value);
        }

        [HttpGet("GetApprovedReservations")]
        public IActionResult GetApprovedReservations()
        {
            var value = _context.Reservations.Where(x => x.ReservationStatus == "Onaylandı").Count();
            return Ok(value);
        }

    }
}
