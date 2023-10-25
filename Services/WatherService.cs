using System.Text.Json;
using ApiAvocados.Dto;
using ApiAvocados.Models;

namespace ApiAvocados.Services;



public class WhaterService : IWhaterService
{
  public string Prueba()
  {
    return "prueba service";
  }
}

public interface IWhaterService
{
  string Prueba();
}
