namespace Photo_Gallery_Web_API.Dtos.GalleryDtos;

public class PictureUpload
{
    public string AlbumName { get; set; }
    public IFormFile PictureFile { get; set; }
}
