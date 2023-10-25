using ApiAvocados.Dto;
using ApiAvocados.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAvocados.Services;
public class OrderService : IOrderService
{
  private readonly ApiContext _dbContext;
  public OrderService(ApiContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<string?> CreateOrder(OrderDto orderDto, List<OrderItemDto> ordenItemsDto)
  {
    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
    {
      try
      {
        var orderId = Guid.NewGuid();

        await _dbContext.Order.AddAsync(new OrderEntity
        {
          Id = orderId,
          Date = orderDto.Date ?? new DateTime(),
          TotalPrice = orderDto.TotalPrice ?? 0,
          UserId = orderDto.UserId
        });
        await _dbContext.SaveChangesAsync();



        List<OrdenItemEntity> orderItems = ordenItemsDto.Select(dto =>
        {
          var orderItem = MaptoOrderItem(dto, orderId);
          return orderItem;
        }).ToList();

        await _dbContext.OrdenItems.AddRangeAsync(orderItems!);
        await _dbContext.SaveChangesAsync();
        transaction.Commit();
        return "Creado correctamente";

      }
      catch (System.Exception)
      {
        transaction.Rollback();
        return null;
      }
    }
  }

  public async void RemoveOrder(Guid orderId)
  {
    var order = await _dbContext.Order.Include(o => o.Items).SingleOrDefaultAsync(o => o.Id == orderId);

    if (order != null)
    {
      foreach (var item in order.Items!)
      {
        var avocado = await _dbContext.Avocados.FindAsync(item.AvocadoId);

        if (avocado != null)
        {
          avocado.Stock += item.Quantity;
        }
      }
      _dbContext.Order.Remove(order);
      _dbContext.SaveChanges();
    }

  }
  public async Task<List<OrderEntity>> GetOrderByUser(Guid userId)
  {
    var orderWithItems = await _dbContext.Order
      .Where(order => order.UserId == userId)
      .Include(order => order.Items)
      .ToListAsync();

    return orderWithItems;
  }

  private OrdenItemEntity MaptoOrderItem(OrderItemDto orderItemDto, Guid orderId)
  {
    var orderItem = new OrdenItemEntity()
    {
      Id = Guid.NewGuid(),
      AvocadoId = orderItemDto.AvocadoId,
      Quantity = orderItemDto.Quantity,
      OrderId = orderId
    };

    var avocado = _dbContext.Avocados.Find(orderItemDto.AvocadoId);

    if (avocado != null && avocado.Stock >= orderItemDto.Quantity)
    {
      avocado.Stock -= orderItemDto.Quantity;
      _dbContext.SaveChanges();
      return orderItem;
    }
    else
    {
      throw new Exception("Ocurrió un error en Mi Método");
    }
  }
}

public interface IOrderService
{
  Task<string?> CreateOrder(OrderDto orderDto, List<OrderItemDto> ordenItemsDto);
  void RemoveOrder(Guid orderId);
  Task<List<OrderEntity>> GetOrderByUser(Guid userId);
}

