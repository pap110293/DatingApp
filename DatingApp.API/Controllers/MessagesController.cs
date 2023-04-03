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
        [HttpGet("{id}", Name = "GetMessage")]
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

            var messageToReturn = _mapper.Map<MessageToReturnDto>(messageFromRepo);

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
                message = await _messageRepo.GetMessage(message.Id);
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
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

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(long userId, long recipientId)
        {
            if (!CheckUserId(userId))
            {
                return Unauthorized();
            }
            var messageFromRepo = await _messageRepo.GetMessagesThread(userId, recipientId);
            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);
            return Ok(messageThread);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(long id, long userId)
        {
            if (!CheckUserId(userId))
            {
                return Unauthorized();
            }

            var messageFromRepo = await _messageRepo.GetMessage(id);
            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _messageRepo.Delete(messageFromRepo);

            if (await _messageRepo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("Remove the message failed on save");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(long userId, long id)
        {
            if (!CheckUserId(userId))
            {
                return Unauthorized();
            }

            var message = await _messageRepo.GetMessage(id);

            if (message.RecipientId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.DateRead = DateTime.Now;
            await _messageRepo.SaveAll();
            return NoContent();
        }
    }
}