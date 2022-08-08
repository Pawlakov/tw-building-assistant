namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;

public readonly record struct Religion(int Id, string Name, Effect EffectWhenState, ImmutableArray<Income> IncomesWhenState, int StateInfluenceWhenState);