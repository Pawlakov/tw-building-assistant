namespace TWBuildingAssistant.Presentation.Models;

public record struct Settings(int ProvinceId, int FertilityDrop, int TechnologyTier, bool UseAntilegacyTechnologies, int ReligionId, int FactionId, int WeatherId, int SeasonId, int DifficultyId, int TaxId, int PowerLevelId, int CorruptionRate, int PiracyRate);