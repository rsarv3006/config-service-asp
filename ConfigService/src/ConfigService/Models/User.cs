using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfigService.Models;

public enum UserRole
{
  Admin,
  User
}

public enum UserStatus
{
  Active,
  Inactive
}

[Table("Users")]
public class User(UserRole role, string appName)
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public Guid Id { get; set; }

  [Required]
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  [Required]
  public UserRole Role { get; set; } = role;

  [Required]
  [MaxLength(120)]
  public string AppName { get; set; } = appName;

  [Required]
  public UserStatus Status { get; set; } = UserStatus.Active;
}
