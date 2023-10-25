namespace ApiAvocados.Dto;
public class AvocadoDto
{
  public string Name { get; set; } = string.Empty;
  public string Sku { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public string Image { get; set; } = string.Empty;
  public int Stock { get; set; }
  public AttributeDto Attributes { get; set; } = new();
}
