namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;

public record class BuildingLevel(int Id, string Name, Effect Effect, ImmutableArray<Income> Incomes, ImmutableArray<Influence> Influences);