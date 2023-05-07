using DatingApp.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers;

public class BuggyController : BaseApiController
{
    private readonly DataContext _context;

    public BuggyController(DataContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("auth")]
    public IActionResult GetSecret()
    {
        return Ok("secret text");
    }

    [HttpGet("not-found")]
    public IActionResult GetNotFound()
    {
        var thing = _context.AppUsers.Find(-1);
        if (thing is null) return NotFound();
        return Ok(thing);
    }


    [HttpGet("server-error")]
    public IActionResult GetServerError()
    {
        throw new NotImplementedException();
    }

    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("this was not a good request");
    }
}