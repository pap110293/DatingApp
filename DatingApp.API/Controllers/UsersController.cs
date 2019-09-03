using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(){
            var users = await _userRepo.GetAll();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(int id){
            var user = await _userRepo.Get(i => i.Id == id);
            var usersToReturn = _mapper.Map<UserForDetailDto>(user);

            if(user != null)
                return Ok(usersToReturn);
            
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdate){
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _userRepo.GetUser(id);
            _mapper.Map(userForUpdate, userFromRepo);
            if(await _userRepo.SaveAll())
            {
                var userToReturn = _mapper.Map<UserForDetailDto>(userFromRepo);
                return Ok(userToReturn);
            }

            throw new Exception($"Updating user {id} failed on save");
        }
    }
}