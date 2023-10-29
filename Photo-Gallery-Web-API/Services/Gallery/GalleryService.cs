    using Data.Context;
    using Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using Photo_Gallery_Web_API.Dtos.GalleryDtos;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    namespace Photo_Gallery_Web_API.Services.Gallery
    {
        public class GalleryService : IGalleryService
        {
            private readonly string _rootFolderPath;
            private readonly DataContext _context;
            private readonly string[] acceptedPictureFormats = { ".jpg", ".jpeg", ".png", ".gif" };

            public GalleryService(IConfiguration configuration, DataContext context)
            {
                _rootFolderPath = configuration["PhotoStorage:Root"];
                _context = context;
            }

            public async Task CreateFolderForUserAsync(User user)
            {
                string folderName = user.UserId.ToString();
                string folderPath = Path.Combine(_rootFolderPath, folderName);

                if (Directory.Exists(folderPath))
                {
                    throw new InvalidOperationException("Folder already exists for the user.");
                }

                await Task.Run(() => Directory.CreateDirectory(folderPath));
            }

            public async Task CreateAlbumInsideUserFolderAsync(Guid userId, string albumName)
            {
                string userFolderName = userId.ToString();
                string userFolderPath = Path.Combine(_rootFolderPath, userFolderName);
                string albumFolderPath = Path.Combine(userFolderPath, albumName);

                if (!Directory.Exists(userFolderPath))
                {
                    throw new InvalidOperationException("User folder does not exist.");
                }

                if (Directory.Exists(albumFolderPath))
                {
                    throw new InvalidOperationException("Folder already exists inside the user's folder.");
                }

                // Check if an album with the same name already exists for the user
                if (_context.Albums.Any(a => a.CreatedByUserId == userId && a.Name == albumName))
                {
                    throw new InvalidOperationException("An album with the same name already exists for the user.");
                }

                await Task.Run(() => Directory.CreateDirectory(albumFolderPath));

                var album = new Album
                {
                    Name = albumName,
                    Path = albumFolderPath,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Albums.Add(album);
                await _context.SaveChangesAsync();
            }

            public async Task CreatePictureInAlbumAsync(Guid userId, PictureUpload pictureDto)
            {
                string userFolderName = userId.ToString();
                string userFolderPath = Path.Combine(_rootFolderPath, userFolderName);
                string albumFolderPath = Path.Combine(userFolderPath, pictureDto.AlbumName);

                if (!Directory.Exists(userFolderPath) || !Directory.Exists(albumFolderPath))
                {
                    throw new InvalidOperationException("User or album folder does not exist.");
                }

                if (pictureDto.PictureFile == null ||
                    !acceptedPictureFormats.Any(format => pictureDto.PictureFile.FileName.EndsWith(format, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException("Invalid picture format.");
                }

                string uniqueFileName = $"{DateTime.Now.Ticks}{Path.GetExtension(pictureDto.PictureFile.FileName)}";
                string pictureFilePath = Path.Combine(albumFolderPath, uniqueFileName);

                using (var stream = new FileStream(pictureFilePath, FileMode.Create))
                {
                    await pictureDto.PictureFile.CopyToAsync(stream);
                }

                var picture = new Picture
                {
                    AlbumId = _context.Albums.Single(a => a.Name == pictureDto.AlbumName && a.CreatedByUserId == userId).Id,
                    Path = pictureFilePath,
                    Likes = 0,
                    Dislikes = 0,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Pictures.Add(picture);
                await _context.SaveChangesAsync();
            }

            public async Task DeletePictureAsync(Guid photoId, Guid userId)
            {
                var photo = await _context.Pictures.FindAsync(photoId);

                if (photo == null)
                {
                    throw new InvalidOperationException("Photo not found.");
                }

                if (photo.CreatedByUserId != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to delete this photo.");
                }

                if (File.Exists(photo.Path))
                {
                    File.Delete(photo.Path);
                }

                _context.Pictures.Remove(photo);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAlbumAsync(Guid userId, string albumName)
            {
                string userFolderName = userId.ToString();
                string albumFolderPath = Path.Combine(_rootFolderPath, userFolderName, albumName);

                if (!Directory.Exists(albumFolderPath))
                {
                    throw new InvalidOperationException("Album not found.");
                }

                var pictureFiles = Directory.GetFiles(albumFolderPath);

                foreach (var pictureFile in pictureFiles)
                {
                    string pictureFileName = Path.GetFullPath(pictureFile);

                    var picture = await _context.Pictures.FirstOrDefaultAsync(p => p.Path == pictureFileName);
                    if (picture != null)
                    {
                        _context.Pictures.Remove(picture);
                    }

                    File.Delete(pictureFile);
                }
                var album = await _context.Albums
                .Where(a => a.CreatedByUserId == userId && a.Name == albumName)
                .FirstOrDefaultAsync();

                if (album != null)
                {
                    _context.Albums.Remove(album);
                }

                Directory.Delete(albumFolderPath);

                await _context.SaveChangesAsync();
            }

        public async Task<IEnumerable<Album>> GetAllMyAlbums(Guid userId)
        {
            try
            {
                var userAlbums = await _context.Albums
                    .Where(a => a.CreatedByUserId == userId)
                    .OrderBy(a => a.Name)
                    .ToListAsync();

                return userAlbums;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IEnumerable<Picture> GetAllMyPicturesFromAlbum(Guid userId, string albumName)
            {
                string userFolderName = userId.ToString();
                string albumFolderPath = Path.Combine(_rootFolderPath, userFolderName, albumName);

                if (!Directory.Exists(albumFolderPath))
                {
                    throw new InvalidOperationException("Album folder does not exist.");
                }

                var pictures = _context.Pictures
                    .Where(p => p.CreatedByUserId == userId && p.Path.StartsWith(albumFolderPath))
                    .ToArray();

                return pictures;
            }
        }
    }
