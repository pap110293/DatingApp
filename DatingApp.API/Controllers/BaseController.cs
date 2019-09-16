using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected bool CheckUserId(long userId)
        {
            var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId == currentUserId;
        }
    }
}