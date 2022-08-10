namespace TWBuildingAssistant.Domain.StateModels;

using System.Collections.Immutable;

public readonly record struct BuildingLibraryEntry(BuildingLibraryEntryDescriptor Descriptor, ImmutableArray<BuildingBranch> BuildingBranches);