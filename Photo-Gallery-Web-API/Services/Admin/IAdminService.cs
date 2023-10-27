using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Photo_Gallery_Web_API.Services.Admin;

public interface IAdminService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<Album>> GetAlbumsByUserId(Guid userId);
    Task<IEnumerable<Picture>> GetPicturesByAlbumId(Guid albumId);
    Task<bool> DeleteAlbumByNameAsync(string albumName);
    Task<bool> DeletePictureByIdAsync(Guid pictureId);
}
