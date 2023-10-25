using System.Text.Json;
using ApiAvocados.Dto;
using ApiAvocados.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAvocados;

public class JsonData
{
  public List<AvocadoDto> data { get; set; } = new();
}

public class ApiContext : DbContext
{
  public DbSet<UserEntity> Users { get; set; }
  public DbSet<AvocadoEntity> Avocados { get; set; }
  public DbSet<AttributeEntity> Attributes { get; set; }
  public DbSet<OrderEntity> Order { get; set; }
  public DbSet<OrdenItemEntity> OrdenItems { get; set; }

  public ApiContext(DbContextOptions<ApiContext> options) : base(options)
  {
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseNpgsql("Host=localhost;Username=root;Password=123456;Database=db_avocados");
    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {


    modelBuilder.Entity<UserEntity>(e =>
    {
      e.ToTable("User");
      e.HasKey(p => p.Id);
      e.Property(p => p.Name).IsRequired().HasMaxLength(100);
      e.Property(p => p.Email).IsRequired().HasMaxLength(200);
      e.Property(p => p.PasswordHash).IsRequired().HasMaxLength(200);
      e.Property(p => p.Avatar).IsRequired(false).HasMaxLength(200);
      e.Property(p => p.Roles).HasDefaultValue(new List<UserRole> { UserRole.Customer });

      e.HasMany(p => p.Orders).WithOne(p => p.User);

      e.HasIndex(p => p.Email).IsUnique();
    });

    modelBuilder.Entity<AvocadoEntity>(e =>
    {
      e.ToTable("Avocado");
      e.HasKey(p => p.Id);
      e.Property(p => p.Name).IsRequired();
      e.Property(p => p.Sku).IsRequired();
      e.Property(p => p.Stock).IsRequired();
      e.Property(p => p.Image).IsRequired();
      e.Property(p => p.Price)
        .IsRequired()
        .HasPrecision(18, 2);

      e.HasOne(p => p.Attributes)
        .WithOne(p => p.Avocado)
        .HasForeignKey<AvocadoEntity>(p => p.AttributeId)
        .IsRequired();
    });

    modelBuilder.Entity<AttributeEntity>(e =>
    {
      e.ToTable("Attribute");
      e.HasKey(p => p.Id);
      e.Property(p => p.Shape).IsRequired();
      e.Property(p => p.Hardiness).IsRequired();
      e.Property(p => p.Description).IsRequired();
      e.Property(p => p.Taste).IsRequired();
    });

    modelBuilder.Entity<OrderEntity>(e =>
    {
      e.ToTable("Order");

      e.HasKey(p => p.Id);
      e.Property(p => p.TotalPrice).IsRequired(false);
      e.Property(p => p.Date).IsRequired(false);

      e.HasOne(p => p.User).WithMany(u => u.Orders).HasForeignKey(p => p.UserId);
      e.HasMany(o => o.Items).WithOne(oi => oi.Order).OnDelete(DeleteBehavior.Cascade);

    });

    modelBuilder.Entity<OrdenItemEntity>(e =>
    {
      e.ToTable("OrdenItem");
      e.HasKey(p => p.Id);
      e.Property(oi => oi.Quantity).IsRequired(true);
      e.HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId);
      e.HasOne(oi => oi.Avocado).WithMany(o => o.OrdenItems).HasForeignKey(oi => oi.AvocadoId);

    });

    var json = File.ReadAllText(Directory.GetCurrentDirectory() + "/data.json");

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };


    JsonData jsonData = JsonSerializer.Deserialize<JsonData>(json, options)!;

    foreach (var avocadoData in jsonData.data)
    {
      var avocadoId = Guid.NewGuid();
      var attributeId = Guid.NewGuid();

      modelBuilder.Entity<AvocadoEntity>().HasData(
          new AvocadoEntity
          {
            Id = avocadoId, // Genera un nuevo Id único
            Name = avocadoData.Name,
            Sku = avocadoData.Sku,
            Stock = avocadoData.Stock,
            Image = avocadoData.Image,
            Price = avocadoData.Price,
            AttributeId = attributeId
            // Otras propiedades
          });

      modelBuilder.Entity<AttributeEntity>().HasData(
          new AttributeEntity
          {
            Id = attributeId,
            Description = avocadoData.Attributes.Description,
            Shape = avocadoData.Attributes.Shape,
            Hardiness = avocadoData.Attributes.Hardiness,
            Taste = avocadoData.Attributes.Taste,
            // Otras propiedades
          });
    }

    base.OnModelCreating(modelBuilder);
  }
}
