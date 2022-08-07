namespace TWBuildingAssistant.Domain;

public readonly record struct ProvinceState(RegionState[] Regions, int Food, int PublicOrder, int ReligiousOsmosis, int ResearchRate, int Growth, double Wealth);