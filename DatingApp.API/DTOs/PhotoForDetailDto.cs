using System;

namespace DatingApp.API.DTOs
{
    public class PhotoForDetailDto
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Discription { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public bool IsApproved { get; set; }
    }
}