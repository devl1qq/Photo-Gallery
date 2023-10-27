﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Photo_Gallery_Web_API.Services.Admin;

namespace Photo_Gallery_Web_API.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _adminService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("albums/{userId}")]
        public async Task<IActionResult> GetAlbumsByUserId(Guid userId)
        {
            try
            {
                var albums = await _adminService.GetAlbumsByUserId(userId);
                return Ok(albums);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pictures/{albumId}")]
        public async Task<IActionResult> GetPicturesByAlbumId(Guid albumId)
        {
            try
            {
                var pictures = await _adminService.GetPicturesByAlbumId(albumId);
                return Ok(pictures);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("album/{albumName}")]
        public async Task<IActionResult> DeleteAlbumByName(string albumName)
        {
            try
            {
                bool result = await _adminService.DeleteAlbumByNameAsync(albumName);
                if (result)
                {
                    return Ok("Album deleted successfully.");
                }
                return BadRequest("Failed to delete the album.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("picture/{pictureId}")]
        public async Task<IActionResult> DeletePictureById(Guid pictureId)
        {
            try
            {
                bool result = await _adminService.DeletePictureByIdAsync(pictureId);
                if (result)
                {
                    return Ok("Picture deleted successfully.");
                }
                return BadRequest("Failed to delete the picture.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
