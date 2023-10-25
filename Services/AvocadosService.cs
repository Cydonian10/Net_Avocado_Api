using ApiAvocados.Dto;
using ApiAvocados.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAvocados.Services;
public class AvocadoService : IAvocadosService
{
  public readonly ApiContext _dbContext;
  public AvocadoService(ApiContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<List<AvocadoEntity>> GetAvocados()
  {
    return await _dbContext.Avocados!.Include(a => a.Attributes).ToListAsync();
  }

  public async Task<List<AvocadoEntity>> AddAvocado(AvocadoDto avocado)
  {
    var avocadoId = new Guid();
    var attributeId = new Guid();

    var newAvocado = new AvocadoEntity()
    {
      Id = avocadoId,
      Image = avocado.Image,
      Price = avocado.Price,
      Stock = avocado.Stock,
      Sku = avocado.Sku,
      Name = avocado.Name,
    };

    var newAttribute = new AttributeEntity()
    {
      Id = attributeId,
      Description = avocado.Attributes.Description,
      Hardiness = avocado.Attributes.Hardiness,
      Shape = avocado.Attributes.Shape,
      Taste = avocado.Attributes.Taste,
    };

    newAvocado.Attributes = newAttribute;

    await _dbContext.Avocados.AddAsync(newAvocado);
    await _dbContext.SaveChangesAsync();

    return await _dbContext.Avocados.Include(a => a.Attributes).ToListAsync();
  }

  public async Task<AvocadoEntity?> GetAvocado(Guid guid)
  {
    var maybeAvocado = await _dbContext.Avocados
    .Include(a => a.Attributes)
    .FirstOrDefaultAsync(a => a.Id == guid);

    if (maybeAvocado == null)
    {
      return null;
    }

    return maybeAvocado;
  }
}


public interface IAvocadosService
{
  Task<List<AvocadoEntity>> GetAvocados();
  Task<AvocadoEntity?> GetAvocado(Guid guid);
  Task<List<AvocadoEntity>> AddAvocado(AvocadoDto avocado);
}


