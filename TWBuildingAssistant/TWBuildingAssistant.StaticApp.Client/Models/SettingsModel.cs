namespace TWBuildingAssistant.StaticApp.Client.Models;

using System.ComponentModel.DataAnnotations;

public class SettingsModel
{
    [Required]
    public string? ProvinceId { get; set; }

    [Required]
    public string? FactionId { get; set; }

    [Required]
    public string? RelligionId { get; set; }

    [Required]
    public string? DifficultyId { get; set; }

    [Required]
    public string? TaxId { get; set; }

    [Required]
    public string? PowerLevelId { get; set; }

    [Required]
    public string? SeasonId { get; set; }

    [Required]
    public string? WeatherId { get; set; }

    [Required]
    public int? FertilityDrop { get; set; }

    [Required]
    public int? TechnologyTier { get; set; }

    [Required]
    [Range(1,99)]
    public int? Corruption { get; set; }

    [Required]
    [Range(1, 99)]
    public int? Piracy { get; set; }

    public bool UseAntilegacy { get; set; }
}
