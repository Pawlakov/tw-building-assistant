namespace TWBuildingAssistant.Presentation.Models;

public record struct Settings(string ProvinceId, int FertilityDrop, int TechnologyTier, bool UseAntilegacyTechnologies, string ReligionId, string FactionId, int WeatherId, int SeasonId, int DifficultyId, int TaxId, int PowerLevelId, int CorruptionRate, int PiracyRate);