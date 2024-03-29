using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.API.Models
{
    public class Photo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Url { get; set; }
        public string Discription { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public bool IsApproved { get; set; }
    }
}