using DatingApp.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers;

public class UsersController : BaseApiController
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.AppUsers.ToListAsync();
        
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.AppUsers.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }
}