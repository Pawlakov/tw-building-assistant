namespace TWBuildingAssistant.Domain.StateModels;

using System.Collections.Immutable;

public readonly record struct SeekerSettingsRegion(ImmutableArray<(BuildingBranch Branch, BuildingLevel Level)> Predefined, ImmutableArray<SlotDescriptor> Slots);