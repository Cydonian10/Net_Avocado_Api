using System.Security.Claims;
using ApiAvocados.Dto;
using ApiAvocados.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAvocados.Controllers;

[ApiController]
[Route("order")]
public class OrderController : ControllerBase
{
  private readonly IOrderService _orderService;
  public OrderController(IOrderService orderService)
  {
    _orderService = orderService;
  }

  [HttpPost]
  public async Task<IActionResult> CreateOrder([FromBody] PreOrderDto preOrderDto)
  {
    var resul = await _orderService.CreateOrder(preOrderDto.Order!, preOrderDto.OrderItems!);

    if (resul != null)
    {
      return Ok(new
      {
        message = "Order Creado correctamente"
      });
    }

    return BadRequest(new
    {
      message = "Error interno"
    });
  }

  [HttpGet("orders-user")]
  [Authorize(Policy = "All")]
  public async Task<IActionResult> GetOrderWithItems()
  {
    var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
    var userId = Guid.Parse(userIdentity!.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)!.Value);

    var orderwithItem = await _orderService.GetOrderByUser(userId);

    return Ok(new
    {
      message = "All orders by user",
      data = orderwithItem
    });
  }
}
