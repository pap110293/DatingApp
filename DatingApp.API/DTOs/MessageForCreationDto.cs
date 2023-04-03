using System;

namespace DatingApp.API.DTOs
{
    public class MessageForCreationDto
    {
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }
        public MessageForCreationDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}