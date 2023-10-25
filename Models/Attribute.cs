using System.Text.Json.Serialization;

namespace ApiAvocados.Models;
public class AttributeEntity
{
  public Guid Id { get; set; }
  public string Description { get; set; } = string.Empty;
  public string Shape { get; set; } = string.Empty;
  public string Hardiness { get; set; } = string.Empty;
  public string Taste { get; set; } = string.Empty;

  [JsonIgnore]
  public virtual AvocadoEntity? Avocado { get; set; }
}
