namespace TWBuildingAssistant.Client.Models;

using System.ComponentModel.DataAnnotations;

public class SettingsModel
{
    [Required]
    public int? ProvinceId { get; set; }

    [Required]
    public int? FactionId { get; set; }

    [Required]
    public int? RelligionId { get; set; }

    [Required]
    public int? DifficultyId { get; set; }

    [Required]
    public int? TaxId { get; set; }

    [Required]
    public int? PowerLevelId { get; set; }

    [Required]
    public int? SeasonId { get; set; }

    [Required]
    public int? WeatherId { get; set; }

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
