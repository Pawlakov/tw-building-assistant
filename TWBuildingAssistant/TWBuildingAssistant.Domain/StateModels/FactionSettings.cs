namespace TWBuildingAssistant.Domain.StateModels;

public readonly record struct FactionSettings(int FertilityDrop, int TechnologyTier, bool UseAntilegacyTechnologies, int ReligionId);
