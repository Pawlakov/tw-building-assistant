namespace TWBuildingAssistant.Domain.StateModels;

using System.Collections.Immutable;

public readonly record struct EffectSet(in Effect Effect, in ImmutableArray<Income> Incomes, in ImmutableArray<Influence> Influences);