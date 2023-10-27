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

    public async Task<bool> LikePhoto(Guid userId, Guid photoId)
    {
        try
        {
            var interaction = await _context.Interactions
                .FirstOrDefaultAsync(upi => upi.UserId == userId && upi.PictureId == photoId);

            if (interaction == null)
            {
                interaction = new UserPictureInteraction
                {
                    UserId = userId,
                    PictureId = photoId,
                    IsLiked = true,
                    IsDisliked = false,
                    InteractionTime = DateTime.UtcNow
                };

                _context.Interactions.Add(interaction);

                var picture = await _context.Pictures
                    .FirstOrDefaultAsync(p => p.Id == photoId);

                if (picture != null)
                {
                    picture.Likes++;
                    _context.Pictures.Update(picture);
                }
            }
            else
            {
                interaction.IsLiked = !interaction.IsLiked;
                interaction.IsDisliked = false;

                var picture = await _context.Pictures
                    .FirstOrDefaultAsync(p => p.Id == photoId);

                if (picture != null)
                {
                    if (interaction.IsLiked)
                        picture.Likes++;
                    else
                        picture.Likes--;

                    _context.Pictures.Update(picture);
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

    public async Task<bool> DislikePhoto(Guid userId, Guid photoId)
    {
        try
        {
            var interaction = await _context.Interactions
                .FirstOrDefaultAsync(upi => upi.UserId == userId && upi.PictureId == photoId);

            if (interaction == null)
            {
                interaction = new UserPictureInteraction
                {
                    UserId = userId,
                    PictureId = photoId,
                    IsLiked = false,
                    IsDisliked = true,
                    InteractionTime = DateTime.UtcNow
                };

                _context.Interactions.Add(interaction);

                var picture = await _context.Pictures
                    .FirstOrDefaultAsync(p => p.Id == photoId);

                if (picture != null)
                {
                    picture.Dislikes++;
                    _context.Pictures.Update(picture);
                }
            }
            else
            {
                interaction.IsLiked = false;
                interaction.IsDisliked = !interaction.IsDisliked;

                var picture = await _context.Pictures
                    .FirstOrDefaultAsync(p => p.Id == photoId);

                if (picture != null)
                {
                    if (interaction.IsDisliked)
                        picture.Dislikes++;
                    else
                        picture.Dislikes--;

                    _context.Pictures.Update(picture);
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
    public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
    {
        try
        {
            var albums = await _context.Albums
                .OrderBy(a => a.Name)
                .ToListAsync();

            return albums;
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately
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

}
