using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Photo_Gallery_Web_API.Services.Admin;

public class AdminService : IAdminService
{
    private readonly DataContext _context;
    private readonly string _rootFolderPath;

    public AdminService(DataContext context, IConfiguration configuration)
    {
        _rootFolderPath = configuration["PhotoStorage:Root"];
        _context = context;
    }
    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            var users = await _context.Users.ToArrayAsync();
            return users;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<Album>> GetAlbumsByUserId(Guid userId)
    {
        try
        {
            var albums = await _context.Albums
                .Where(a => a.CreatedByUserId == userId)
                .ToListAsync();
            return albums;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<Picture>> GetPicturesByAlbumId(Guid albumId)
    {
        try
        {
            var pictures = await _context.Pictures
                .Where(p => p.AlbumId == albumId)
                .ToListAsync();
            return pictures;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> DeleteAlbumByNameAsync(string albumName)
    {
        var album = await _context.Albums.FirstOrDefaultAsync(a => a.Name == albumName);

        if (album == null)
        {
            throw new InvalidOperationException("Album not found.");
        }

        var albumFolderPath = Path.Combine(_rootFolderPath, album.CreatedByUserId.ToString(), albumName);

        if (Directory.Exists(albumFolderPath))
        {
            var pictureFiles = Directory.GetFiles(albumFolderPath);

            foreach (var pictureFile in pictureFiles)
            {
                string pictureFileName = Path.GetFullPath(pictureFile);

                var picture = await _context.Pictures.FirstOrDefaultAsync(p => p.Path == pictureFileName);
                if (picture != null)
                {
                    _context.Pictures.Remove(picture);
                    await _context.SaveChangesAsync();
                }

                File.Delete(pictureFile);
            }

            Directory.Delete(albumFolderPath);
        }

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePictureByIdAsync(Guid pictureId)
    {
        var picture = await _context.Pictures.FindAsync(pictureId);

        if (picture == null)
        {
            throw new InvalidOperationException("Picture not found.");
        }

        var picturePath = picture.Path;

        if (File.Exists(picturePath))
        {
            File.Delete(picturePath);
        }

        _context.Pictures.Remove(picture);
        await _context.SaveChangesAsync();
        return true;
    }

}
