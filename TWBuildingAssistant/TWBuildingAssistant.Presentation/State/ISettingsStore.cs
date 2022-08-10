namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsStore
{
    Settings Settings { get; set; }

    Effect Effect { get; set; }

    ImmutableArray<Income> Incomes { get; set; }

    ImmutableArray<Influence> Influences { get; set; }
}