using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepo;
        private readonly ILikeRepository _likeRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepo, IMapper mapper, ILikeRepository likeRepo)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _likeRepo = likeRepo;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _userRepo.GetUser(currentUserId);
            
            userParams.UserId = userFromRepo.Id;

            var pagedUsers = await _userRepo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(pagedUsers);

            Response.AddPagination(pagedUsers.CurrentPage,pagedUsers.PageSize,pagedUsers.TotalCount, pagedUsers.TotalPages);

            return Ok(usersToReturn);
        }

        // GET: api/users/{id}
        [HttpGet("{id}",Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id){
            var user = await _userRepo.Get(i => i.Id == id);
            var usersToReturn = _mapper.Map<UserForDetailDto>(user);

            if(user != null)
                return Ok(usersToReturn);
            
            return NotFound();
        }

        // PUT: api/users/{id}
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

        [HttpPost("{id}/like/{recepientId}")]
        public async Task<IActionResult> LikeUser(long id, long recepientId)
        {
            if(!CheckUserId(id))
            {
                return Unauthorized();
            }

            var like =  await _likeRepo.GetLike(id, recepientId);
            if(like != null)
                return BadRequest("You already like this user");

            if(!await _userRepo.Existed(id) || !await _userRepo.Existed(recepientId))
                return NotFound();

            var newLike = new Like{
                LikerId = id,
                LikeeId = recepientId
            };

            _likeRepo.Add(newLike);

            if(await _likeRepo.SaveAll())
            {
                return Ok();
            }
            
            return BadRequest("Failed to like user");
        }
    }
}