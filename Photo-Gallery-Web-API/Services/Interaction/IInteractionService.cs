using Data.Entities;

namespace Photo_Gallery_Web_API.Services.Interaction;

public interface IInteractionService
{
    Task<bool> LikePhoto(Guid userId, Guid photoId);
    Task<bool> DislikePhoto(Guid userId, Guid photoId);
    Task<IEnumerable<Album>> GetAllAlbumsAsync();
    Task<IEnumerable<Picture>> GetPhotosFromAlbumAsync(Guid albumId, int page, int pageSize);
}
