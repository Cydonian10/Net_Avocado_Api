
namespace ApiAvocados.Services;
public class ImageUploadService : IImageUploadService
{
  public async Task<string?> UploadImage(IFormFile file)
  {

    if (file == null || file.Length <= 0)
    {
      return null;
    }

    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);


    using var stream = new FileStream(filePath, FileMode.Create);
    await file.CopyToAsync(stream);

    return "Image Upload Succeful";

  }

  public async Task<byte[]?> GetImage(string imageName)
  {
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", imageName);

    if (!System.IO.File.Exists(filePath))
    {
      return null;
    }

    var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);

    return imageBytes;
  }
}


public interface IImageUploadService
{
  Task<string?> UploadImage(IFormFile file);
  Task<byte[]?> GetImage(string imageName);
}