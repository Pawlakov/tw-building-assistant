namespace TWBuildingAssistant.HybridActor.Models;

using System.ComponentModel.DataAnnotations;

public class SettingsModel
{
    [Required]
    public string? ProvinceId { get; set; }
}
