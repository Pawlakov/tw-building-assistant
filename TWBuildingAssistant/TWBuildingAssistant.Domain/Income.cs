namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Data.Model;

public readonly record struct Income(IncomeCategory? Category, int Simple, int Percentage, int FertilityDependent);