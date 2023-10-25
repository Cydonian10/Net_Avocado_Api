using ApiAvocados.Models;
using ApiAvocados.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Unicode;
using System.IO;

namespace ApiAvocados.Controllers;


public class JsonData
{
    public List<AvocadoEntity> Data { get; set; } = new List<AvocadoEntity>();
}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private IWhaterService _whaterService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWhaterService whaterService)
    {
        _logger = logger;
        _whaterService = whaterService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("pruba")]
    public ActionResult<string> PruebaSeed()
    {
        var result = _whaterService.Prueba();
        return Ok("hola mundo");
    }


    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length <= 0)
        {
            return BadRequest("No se proporciono ningun archivo o el archivo esta vacio");
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }


        return Ok("Image cargada correctamente");
    }


    [HttpGet("images/{imageName}")]
    public IActionResult GetImage(string imageName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", imageName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("La imagen no se encontro");
        }

        var imageBytes = System.IO.File.ReadAllBytes(filePath);

        return File(imageBytes, "image/jpeg");
    }
}
