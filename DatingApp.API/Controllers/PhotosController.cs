using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserLastActivity))]
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : BaseController
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly IPhotoRepository _photoRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private Cloudinary _cloudinary;

        public PhotosController(IUserRepository userRepo,
                                IMapper mapper,
                                IOptions<CloudinarySettings> cloudinaryConfig,
                                IPhotoRepository photoRepo)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            _photoRepo = photoRepo;
            Account acc = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        }

        // GET: api/users/{userId}/photos/{id}
        [HttpGet("{id}", Name = nameof(GetPhoto))]
        public async Task<IActionResult> GetPhoto(long userId, long id)
        {
            if (!CheckUserId(userId))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _photoRepo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }


        // GET: api/users/{userId}/photos
        [HttpGet]
        public async Task<IActionResult> GetPhotos(long userId)
        {
            if (!CheckUserId(userId))
            {
                return Unauthorized();
            }

            var userFromRepo = await _userRepo.GetUser(userId);
            return Ok(userFromRepo.Photos.Select(p => _mapper.Map<PhotoForReturnDto>(p)));
        }

        // POST: api/users/{userId}/photos
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(long userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (!CheckUserId(userId))
            {
                return Unauthorized();
            }

            var userFromRepo = await _userRepo.GetUser(userId);
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("faces")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;
            }

            userFromRepo.Photos.Add(photo);

            if (await _userRepo.SaveAll())
            {
                var photoForReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtAction(nameof(GetPhoto), new { id = photo.Id }, photoForReturn);
            }

            return BadRequest("Cound not add the photo");
        }

        // POST: api/users/{userId}/photos/{id}/setMain
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(long userId, long id)
        {
            if (!CheckUserId(userId))
                return Unauthorized();

            var user = await _userRepo.GetUser(userId);
            if (!user.Photos.Any(i => i.Id == id))
                return BadRequest();

            var currentMainPhoto = user.Photos.FirstOrDefault(p => p.IsMain);

            if (currentMainPhoto.Id == id)
                return BadRequest("This is already main photo");

            if (currentMainPhoto != null)
                currentMainPhoto.IsMain = false;

            var newMainPhoto = user.Photos.FirstOrDefault(p => p.Id == id);
            if (newMainPhoto != null)
                newMainPhoto.IsMain = true;

            if (await _photoRepo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        // Delete: api/users/{userId}/photos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(long userId, long id)
        {
            if (!CheckUserId(userId))
                return Unauthorized();

            if (!await _photoRepo.Any(userId, id))
                return BadRequest();

            var photoFromRepo = await _photoRepo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");

            if (string.IsNullOrWhiteSpace(photoFromRepo.PublicId))
            {
                _photoRepo.Delete(photoFromRepo);
            }
            else
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);
                var resutl = _cloudinary.Destroy(deleteParams);

                if (resutl.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _photoRepo.Delete(photoFromRepo);
                }
            }

            if (await _photoRepo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete the photo");
        }
    }
}