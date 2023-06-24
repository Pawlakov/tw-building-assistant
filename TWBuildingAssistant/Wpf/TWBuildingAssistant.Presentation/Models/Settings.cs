namespace TWBuildingAssistant.Presentation.Models;

public record struct Settings(string ProvinceId, int FertilityDrop, int TechnologyTier, bool UseAntilegacyTechnologies, string ReligionId, string FactionId, string WeatherId, string SeasonId, int DifficultyId, int TaxId, int PowerLevelId, int CorruptionRate, int PiracyRate);