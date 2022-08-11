namespace TWBuildingAssistant.Domain.StateModels;

using System.Collections.Immutable;

public readonly record struct SeekerSettingsRegion(ImmutableArray<SeekerSettingsSlot> Slots);