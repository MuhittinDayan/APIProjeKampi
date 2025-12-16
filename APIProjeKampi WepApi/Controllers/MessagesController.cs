using APIProjeKampi_WepApi.Context;
using APIProjeKampi_WepApi.Dtos.MessagedDtos;
using APIProjeKampi_WepApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProjeKampi_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;

        public MessagesController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public IActionResult MessageList()
        {
            var value = _context.Messages.ToList();
            return Ok(_mapper.Map<List<ResultMessageDto>>(value));
        }
        [HttpPost]
        public IActionResult CreateMessage(CreateMessageDto createMessageDto)
        {
            var value = _mapper.Map<Message>(createMessageDto);
            _context.Messages.Add(value);
            _context.SaveChanges();
            return Ok("Ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteMessage(int id)
        {
            var value = _context.Messages.Find(id);
            _context.Messages.Remove(value);
            _context.SaveChanges();
            return Ok("Silme işlemi başarılı");
        }
        [HttpPut]
        public IActionResult UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            var values = _mapper.Map<Message>(updateMessageDto);
            _context.Messages.Update(values);
            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarılı");
        }
        [HttpGet("GetMessage")]
        public IActionResult GetMessages(int id)
        {
            var value = _context.Messages.Find(id);
            return Ok(_mapper.Map<GetByIdMessageDto>(value));
        }

        [HttpGet("MessageListByIsReadFalse")]
        public IActionResult MessageListByIsReadFalse()
        {
            var value = _context.Messages.Where(x => x.IsRead).ToList();
            return Ok(value);
        }
    }
}

