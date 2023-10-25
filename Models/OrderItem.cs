using System.Text.Json.Serialization;

namespace ApiAvocados.Models;
public class OrdenItemEntity
{
  public Guid Id { get; set; }
  public Guid OrderId { get; set; }
  public Guid AvocadoId { get; set; }
  public int Quantity { get; set; }

  [JsonIgnore]
  public OrderEntity? Order { get; set; }
  public AvocadoEntity? Avocado { get; set; }

}
