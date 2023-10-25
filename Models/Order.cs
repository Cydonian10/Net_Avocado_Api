using System.Text.Json.Serialization;

namespace ApiAvocados.Models;
public class OrderEntity
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public decimal? TotalPrice { get; set; }
  public DateTime? Date { get; set; }
  public virtual List<OrdenItemEntity>? Items { get; set; }

  [JsonIgnore]
  public UserEntity? User { get; set; }
}

// using (var dbContext = new TuContextoDeBaseDeDatos())
// {
//     using (var transaction = dbContext.Database.BeginTransaction())
//     {
//         try
//         {
//             // Realiza operaciones de base de datos dentro de la transacción
//             dbContext.Entidad1.Add(new Entidad1 { Propiedad1 = "Valor1" });
//             dbContext.Entidad2.Add(new Entidad2 { Propiedad2 = "Valor2" });

//             // Guarda los cambios en la base de datos
//             dbContext.SaveChanges();

//             // Si todas las operaciones se completan con éxito, confirma la transacción
//             transaction.Commit();
//         }
//         catch (Exception)
//         {
//             // Si ocurre un error, deshace la transacción
//             transaction.Rollback();
//         }
//     }
// }