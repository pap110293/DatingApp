using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IPhotoRepository _photoRepository;
        public AdminController (DataContext context, UserManager<User> userManager, RoleManager<Role> roleManager, IPhotoRepository photoRepository) {
            _photoRepository = photoRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        [Authorize (Policy = "RequireAdministrator")]
        [HttpGet ("userWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles () {
            var userList = await _context.Users.OrderBy (i => i.UserName).Select (i => new {
                Id = i.Id,
                    UserName = i.UserName,
                    Roles = (from userRole in i.UserRole join role in _context.Roles on userRole.RoleId equals role.Id select role.Name).ToList ()
            }).ToListAsync ();

            return Ok (userList);
        }

        [Authorize (Policy = "RequireAdministrator")]
        [HttpGet ("allRoles")]
        public async Task<IActionResult> GetAllRoles () {
            var roles = await _roleManager.Roles.OrderBy (r => r.Name).ToListAsync ();
            return Ok (roles.Select (i => i.Name));
        }

        [Authorize (Policy = "RequireAdministrator")]
        [HttpPost ("{userName}/roles/")]
        public async Task<IActionResult> EditRoles (string userName, RoleEditDto roleEditDto) {
            var user = await _userManager.FindByNameAsync (userName);
            var userRoles = await _userManager.GetRolesAsync (user);
            var selectedRoles = roleEditDto.RoleNames ?? new string[] { };
            var result = await _userManager.AddToRolesAsync (user, selectedRoles.Except (userRoles));
            if (!result.Succeeded)
                return BadRequest ("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync (user, userRoles.Except (selectedRoles));
            if (!result.Succeeded)
                return BadRequest ("Failed to remove from roles");

            return Ok (await _userManager.GetRolesAsync (user));
        }

        [Authorize (Policy = "RequireManager")]
        [HttpPost ("photos/{photoId}")]
        public async Task<IActionResult> ApprovePhoto (long photoId) {
            var photo = await _photoRepository.GetPhoto(photoId);
            photo.IsApproved = true;
            await _photoRepository.SaveAll();
            return NoContent();
        }
    }
}