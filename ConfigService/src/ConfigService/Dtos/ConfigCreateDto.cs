using System.Text.Json;

public class ConfigCreateDto
{
  public JsonDocument Config { get; set; }

  public ConfigCreateDto(JsonDocument config)
  {
    Config = config;
  }
}
