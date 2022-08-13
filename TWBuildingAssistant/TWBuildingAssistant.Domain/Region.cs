namespace TWBuildingAssistant.Domain;

using System.Collections.Immutable;
using TWBuildingAssistant.Data.Model;

public readonly record struct Region(int Id, string Name, RegionType RegionType, int? ResourceId, string? ResourceName, ImmutableArray<SlotDescriptor> Slots);