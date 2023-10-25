namespace ApiAvocados.Dto;
public class OrderItemDto
{
  public Guid? OrderId { get; set; }
  public Guid AvocadoId { get; set; }
  public int Quantity { get; set; }

}
