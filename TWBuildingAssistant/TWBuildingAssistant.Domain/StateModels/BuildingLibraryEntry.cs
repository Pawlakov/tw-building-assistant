namespace TWBuildingAssistant.Domain.StateModels;

using System.Collections.Immutable;
using TWBuildingAssistant.Domain;

public readonly record struct BuildingLibraryEntry(Data.FSharp.Models.SlotDescriptor Descriptor, ImmutableArray<BuildingBranch> BuildingBranches);