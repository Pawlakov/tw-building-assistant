namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.StateModels;

public class SettingsStore
    : ISettingsStore
{
    public Settings Settings { get; set; }

    public Effect Effect { get; set; }

    public ImmutableArray<Income> Incomes { get; set; }

    public ImmutableArray<Influence> Influences { get; set; }

    public ImmutableArray<BuildingLibraryEntry> BuildingLibrary { get; set; }
}