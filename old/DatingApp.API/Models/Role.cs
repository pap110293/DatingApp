using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Models
{
    public class Role : IdentityRole<long>
    {
        public virtual ICollection<UserRole> UserRoles{get;set;}
    }
}