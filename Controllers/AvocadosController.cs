using ApiAvocados.Dto;
using ApiAvocados.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiAvocados.Controllers;


[ApiController]
[Route("avocados")]
public class AvocadosController : ControllerBase
{
  private readonly IAvocadosService _avocadosService;
  public AvocadosController(IAvocadosService avocadosService)
  {
    _avocadosService = avocadosService;
  }

  [HttpGet]
  public async Task<ActionResult> GetAvocados()
  {
    var avocados = await _avocadosService.GetAvocados();

    return Ok(new
    {
      message = "Avocados",
      data = avocados
    });
  }

  [HttpGet("{guid}")]
  public async Task<IActionResult> GetAvocado(Guid guid)
  {
    var avocado = await _avocadosService.GetAvocado(guid);

    if (avocado == null)
    {
      return NotFound(new
      {
        message = "Avocado no encontrado"
      });
    }

    return Ok(new
    {
      message = "Avocado",
      data = avocado
    });
  }

  [HttpPost]
  public async Task<ActionResult> AddAvocado([FromBody] AvocadoDto avocado)
  {
    var avocados = await _avocadosService.AddAvocado(avocado);

    return Ok(new
    {
      message = "Add Avocado success",
      data = avocados
    });
  }

}
