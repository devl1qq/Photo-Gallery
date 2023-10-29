    using Data.Entities;

namespace Photo_Gallery_Web_API.Services.Interaction;

public interface IInteractionService
{
    Task<bool> LikeOrDislikePhoto(Guid userId, Guid photoId, bool like);
    Task<IEnumerable<Album>> GetAllAlbumsAsync(int page, int pageSize);
    Task<IEnumerable<Picture>> GetPhotosFromAlbumAsync(Guid albumId, int page, int pageSize);
    Task<UserPictureInteraction> FindInteractionAsync(Guid userId, Guid photoId);
}
