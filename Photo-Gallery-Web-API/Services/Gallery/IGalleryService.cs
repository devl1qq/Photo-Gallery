using Data.Entities;
using Photo_Gallery_Web_API.Dtos.GalleryDtos;

namespace Photo_Gallery_Web_API.Services.Gallery;

public interface IGalleryService
{
    Task CreateFolderForUserAsync(User user);
    Task CreateAlbumInsideUserFolderAsync(Guid userId, string albumName);
    Task CreatePictureInAlbumAsync(Guid userId, PictureUpload pictureDto);
    Task DeletePictureAsync(Guid photoId, Guid userId);
    Task DeleteAlbumAsync(Guid userId, string albumName);
    IEnumerable<string> GetAllMyAlbums(Guid userId);
    IEnumerable<Picture> GetAllMyPicturesFromAlbum(Guid userId, string albumName);
}
