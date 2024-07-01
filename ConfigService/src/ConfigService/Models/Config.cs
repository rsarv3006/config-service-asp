using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ConfigService.Models;

public enum ConfigStatus
{
  Active,
  Inactive
}

[Table("Configs")]
public class Config
{
  public Config(JsonDocument appConfig, string appName, int version, ConfigStatus status)
  {
    CreatedAt = DateTime.UtcNow;
    Status = status;
    AppConfig = appConfig;
    AppName = appName;
    Version = version;
  }

  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public Guid Id { get; set; }

  [Required]
  public DateTime CreatedAt { get; set; }

  [Required]
  [MaxLength(255)]
  public string AppName { get; set; }

  [Required]
  public int Version { get; set; }

  [Required]
  public ConfigStatus Status { get; set; }

  [Required]
  public JsonDocument AppConfig { get; set; }
}
