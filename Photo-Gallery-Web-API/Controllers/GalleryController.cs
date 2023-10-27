using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Photo_Gallery_Web_API.Dtos.GalleryDtos;
using Photo_Gallery_Web_API.Services.Gallery;
using System.Security.Claims;

namespace Photo_Gallery_Web_API.Controllers;

[Route("api/gallery")]
[ApiController]
public class GalleryController : ControllerBase
{
    private readonly IGalleryService _galleryService;

    public GalleryController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }
    [Authorize]
    [HttpPost("create-album-inside-user-folder")]
    public async Task<IActionResult> CreateAlbumInsideUserFolder([FromBody] AlbumRequest albumDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid or missing User ID claim.");
            }

            await _galleryService.CreateAlbumInsideUserFolderAsync(userId, albumDto.AlbumName);
            return Ok("Album created successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [Authorize]
    [HttpPost("upload-picture")]
    public async Task<IActionResult> UploadPicture([FromForm] PictureUpload pictureDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid or missing User ID claim.");
            }
            await _galleryService.CreatePictureInAlbumAsync(userId, pictureDto);
            return Ok("Picture uploaded successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("delete-picture/{photoId}")]
    public async Task<IActionResult> DeletePicture(Guid photoId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid or missing User ID claim.");
            }

            await _galleryService.DeletePictureAsync(photoId, userId);
            return Ok("Picture deleted successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(); 
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("delete-album")]
    public async Task<IActionResult> DeleteAlbum([FromBody] AlbumRequest deleteAlbumDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid or missing User ID claim.");
            }

            await _galleryService.DeleteAlbumAsync(userId, deleteAlbumDto.AlbumName);

            return Ok("Album and associated pictures deleted successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-all-my-albums")]
    public IActionResult GetAllMyAlbums()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid or missing User ID claim.");
            }

            var albumNames = _galleryService.GetAllMyAlbums(userId);

            return Ok(albumNames);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("get-pictures-from-album")]
    public IActionResult GetPicturesFromAlbum([FromBody] AlbumRequest albumDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid or missing User ID claim.");
            }

            var pictures = _galleryService.GetAllMyPicturesFromAlbum(userId, albumDto.AlbumName);

            return Ok(pictures);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}