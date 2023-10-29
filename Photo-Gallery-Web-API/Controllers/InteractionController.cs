using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Photo_Gallery_Web_API.Services.Interaction;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Photo_Gallery_Web_API.Controllers
{
    [Route("api/interaction")]
    [ApiController]
    public class InteractionController : ControllerBase
    {
        private readonly IInteractionService _interactionService;

        public InteractionController(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        [Authorize]
        [HttpPost("like-dislike")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> LikeOrDislikePhoto([FromBody] Guid photoId, [FromQuery] bool like)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return BadRequest("Invalid or missing User ID claim.");
                }

                bool result = await _interactionService.LikeOrDislikePhoto(userId, photoId, like);

                if (result)
                {
                    if (like)
                    {
                        return Ok("Photo liked successfully.");
                    }
                    else
                    {
                        return Ok("Photo disliked successfully.");
                    }
                }

                return BadRequest("Failed to like/dislike the photo.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("other-users-albums")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Album>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetOtherUsersAlbums([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            try
            {
                var albums = await _interactionService.GetAllAlbumsAsync(page, pageSize);

                return Ok(albums);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("other-users-album-photos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Picture>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetPhotosFromOtherUsersAlbum(
        [FromQuery] Guid albumId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            try
            {                
                var photos = await _interactionService.GetPhotosFromAlbumAsync(albumId, page, pageSize);

                return Ok(photos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("find-interaction")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserPictureInteraction))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> FindInteraction([FromQuery] Guid photoId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return BadRequest("Invalid or missing User ID claim.");
                }

                var interaction = await _interactionService.FindInteractionAsync(userId, photoId);

                if (interaction != null)
                {
                    return Ok(interaction);
                }

                return BadRequest("Interaction not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
