using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Photo_Gallery_Web_API.Services.Interaction;

public class InteractionService : IInteractionService
{
    private readonly DataContext _context;

    public InteractionService(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> LikeOrDislikePhoto(Guid userId, Guid photoId, bool like)
    {
        try
        {
            var interaction = await _context.Interactions
                .FirstOrDefaultAsync(upi => upi.UserId == userId && upi.PictureId == photoId);

            var picture = await _context.Pictures
                .FirstOrDefaultAsync(p => p.Id == photoId);

            if (interaction == null)
            {
                interaction = new UserPictureInteraction
                {
                    UserId = userId,
                    PictureId = photoId,
                    IsLiked = like,
                    IsDisliked = !like,
                    InteractionTime = DateTime.UtcNow
                };

                _context.Interactions.Add(interaction);

                if (picture != null)
                {
                    if (like)
                    {
                        picture.Likes++;
                    }
                    else
                    {
                        picture.Dislikes++;
                    }
                    _context.Pictures.Update(picture);
                }
            }
            else
            {
                if (interaction.IsLiked == like)
                {
                    // User is already in the desired state (like or dislike); do nothing or handle as needed.
                }
                else
                {
                    interaction.IsLiked = like;
                    interaction.IsDisliked = !like;

                    if (picture != null)
                    {
                        if (like)
                        {
                            picture.Likes++;
                            picture.Dislikes--;
                        }
                        else
                        {
                            picture.Dislikes++;
                            picture.Likes--;
                        }
                        _context.Pictures.Update(picture);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


    public async Task<IEnumerable<Album>> GetAllAlbumsAsync(int page, int pageSize)
    {
        try
        {
            var albums = await _context.Albums
                .OrderBy(a => a.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize) 
                .ToListAsync();

            return albums;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<Picture>> GetPhotosFromAlbumAsync(Guid albumId, int page, int pageSize)
    {
        try
        {
            var album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == albumId);

            if (album == null)
            {
                throw new InvalidOperationException("Album not found.");
            }

            var pictures = await _context.Pictures
                .Where(p => p.AlbumId == albumId)
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return pictures;
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately
            throw ex;
        }
    }

    public async Task<UserPictureInteraction> FindInteractionAsync(Guid userId, Guid photoId)
    {
        try
        {
            var interaction = await _context.Interactions
                .FirstOrDefaultAsync(upi => upi.UserId == userId && upi.PictureId == photoId);

            return interaction;
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately
            throw ex;
        }
    }

}
