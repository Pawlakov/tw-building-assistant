namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;

public readonly record struct BuildingLevel(int Id, string Name, Effect Effect, ImmutableArray<Income> Incomes, ImmutableArray<Influence> Influences);