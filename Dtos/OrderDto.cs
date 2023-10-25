namespace ApiAvocados.Dto;
public class OrderDto
{
  public Guid UserId { get; set; }
  public decimal? TotalPrice { get; set; }
  public DateTime? Date { get; set; }

}


public class PreOrderDto
{
  public required List<OrderItemDto>? OrderItems { get; set; }
  public required OrderDto? Order { get; set; }
}