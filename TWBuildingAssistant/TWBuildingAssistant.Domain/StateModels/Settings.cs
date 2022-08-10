﻿namespace TWBuildingAssistant.Domain.StateModels;

public readonly record struct Settings(int ProvinceId, int FertilityDrop, int TechnologyTier, bool UseAntilegacyTechnologies, int ReligionId, int FactionId, int WeatherId, int SeasonId, int CorruptionRate);