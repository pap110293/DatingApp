using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    public class FallbackController : Controller
    {
       public IActionResult Index(){
           return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "index.html"),"text/HTML");
       }
    }
}