using System.Text.Json.Serialization;

namespace ApiAvocados.Models;
public class AvocadoEntity
{
  public Guid Id { get; set; }
  public Guid AttributeId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Sku { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public string Image { get; set; } = string.Empty;
  public int Stock { get; set; }
  public virtual AttributeEntity? Attributes { get; set; }
  [JsonIgnore]
  public virtual List<OrdenItemEntity>? OrdenItems { get; set; }

}
