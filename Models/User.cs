
namespace ApiAvocados.Models;
public class UserEntity
{
  public Guid Id { get; set; }
  public string Email { get; set; } = string.Empty;
  public string PasswordHash { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string? Avatar { get; set; }
  public List<UserRole>? Roles { get; set; }
  public virtual List<OrderEntity>? Orders { get; set; }
}


public enum UserRole
{
  [StringValue("admin")]
  Admin,
  [StringValue("customer")]
  Customer
}

[AttributeUsage(AttributeTargets.All)]
public class StringValueAttribute : Attribute
{
  public string Value { get; }

  public StringValueAttribute(string value)
  {
    Value = value;
  }
}
