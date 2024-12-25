namespace TWBuildingAssistant.Actor.Models;

public record struct Settings(string ProvinceId, int FertilityDrop, int TechnologyTier, bool UseAntilegacyTechnologies, string ReligionId, string FactionId, string WeatherId, string SeasonId, string DifficultyId, string TaxId, string PowerLevelId, int CorruptionRate, int PiracyRate);