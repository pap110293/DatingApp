using System.Security.Cryptography;
using System.Text;
using DatingApp.Api.Data;
using DatingApp.Api.DTOs;
using DatingApp.Api.Entities;
using DatingApp.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")] // POST: api/account/register
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (await IsUserExisted(registerDto.Username))
        {
            return BadRequest("Username is taken");
        }
        
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.AppUsers.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        
        return Ok(userDto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == loginDto.Username.ToLower());

        if (user is null) return Unauthorized("Username or Password is invalid");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        
        if (computedHash.Where((t, i) => t != user.PasswordHash[i]).Any())
        {
            return Unauthorized("Username or Password is invalid");
        }

        var userDto = new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        
        return Ok(userDto);
    }

    private async Task<bool> IsUserExisted(string username)
    {
        return await _context.AppUsers.AnyAsync(u => u.UserName == username.ToLower());
    }
}