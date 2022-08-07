namespace TWBuildingAssistant.Domain;

public readonly record struct Effect(int PublicOrder, int RegularFood, int FertilityDependentFood, int ProvincialSanitation, int ResearchRate, int Growth, int Fertility, int ReligiousOsmosis, int RegionalSanitation);