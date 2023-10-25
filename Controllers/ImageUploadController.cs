using ApiAvocados.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiAvocados.Controllers;

[ApiController]
[Route("upload-image")]
public class ImageController : ControllerBase
{
  private readonly IImageUploadService _imageUploadService;
  public ImageController(IImageUploadService imageUploadService)
  {
    _imageUploadService = imageUploadService;
  }

  [HttpPost]
  public async Task<IActionResult> UploadImage(IFormFile file)
  {
    var _message = await _imageUploadService.UploadImage(file);

    if (_message == null)
    {
      return BadRequest(new
      {
        message = "Image Upload file"
      });
    }

    return Ok(new { message = _message });
  }

  [HttpGet("images/{imageName}")]
  public async Task<IActionResult> GetImage(string imageName)
  {
    var imageBytes = await _imageUploadService.GetImage(imageName);

    if (imageBytes == null)
    {
      return NotFound("La imagen no se encontro");

    }

    return File(imageBytes, "image/jpg");
  }

}
