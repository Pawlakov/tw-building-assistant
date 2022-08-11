namespace TWBuildingAssistant.Domain.StateModels;

using System.Collections.Immutable;
using TWBuildingAssistant.Domain;

public readonly record struct BuildingLibraryEntry(SlotDescriptor Descriptor, ImmutableArray<BuildingBranch> BuildingBranches);