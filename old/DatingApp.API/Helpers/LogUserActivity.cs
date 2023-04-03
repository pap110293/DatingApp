using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    public class LogUserLastActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            
            long userId = -1;
            long.TryParse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId);
            if (userId < 0)
                return;

            var userRepo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await userRepo.GetUser(userId);
            user.LastActive = DateTime.Now;
            await userRepo.SaveAll();
        }
    }
}