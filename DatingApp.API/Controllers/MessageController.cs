using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserLastActivity))]
    [Authorize]
    [Route("api/users/{userId}/{[controller]}")]
    [ApiController]
    public class MessagesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userPrepo;

        public MessagesController(IMessageRepository messageRepo, IMapper mapper, IUserRepository userPrepo)
        {
            _userPrepo = userPrepo;
            _messageRepo = messageRepo;
            _mapper = mapper;
        }

        // POST: api/users/{userId}/messages/{id}
        [HttpPost("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(long userId, long messageId)
        {
            if (CheckUserId(userId))
            {
                return Unauthorized();
            }

            var messageFromRepo = await _messageRepo.GetMessage(messageId);

            if (messageFromRepo == null)
            {
                return NotFound();
            }

            var messageToReturn = _mapper.Map<MessageForCreationDto>(messageFromRepo);

            return Ok(messageToReturn);
        }

        // POST: api/users/{userId}/messages
        [HttpPost]
        public async Task<IActionResult> CreateMessage(long userId, MessageForCreationDto messageForCreationDto)
        {
            if (!CheckUserId(userId))
            {
                return Unauthorized();
            }

            messageForCreationDto.SenderId = userId;

            var recipient = await _userPrepo.GetUser(messageForCreationDto.RecipientId);

            if (recipient == null)
            {
                return BadRequest("Could not find user");
            }

            var message = _mapper.Map<Message>(messageForCreationDto);

            _messageRepo.Add(message);

            if (await _messageRepo.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageForCreationDto>(message);
                return CreatedAtRoute("GetMessage", new { id = message.Id }, messageToReturn);
            }

            throw new Exception("Creating the message failed on save");
        }

        // GET: api/users/{userId}/messages
        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(long userId, [FromQuery]MessageParams messageParams)
        {
            if (!CheckUserId(userId))
            {
                return Unauthorized();
            }

            messageParams.UserId = userId;

            var messagesFromRepo = await _messageRepo.GetMessagesForUser(messageParams);
            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);
        }
    }
}