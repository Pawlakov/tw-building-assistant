namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;

public class BuildingLevel
{
    public BuildingLevel(string name, Effect effect, IEnumerable<Income> incomes, Influence influence)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Building level without name.");
        }

        this.Name = name;
        this.Effect = effect;
        this.Incomes = incomes.ToList();
        this.Influence = influence;
    }

    public string Name { get; }

    public Effect Effect { get; }

    public IEnumerable<Income> Incomes { get; }

    public Influence Influence { get; }

    public static BuildingLevel Empty { get; } = new BuildingLevel("Empty", default, Enumerable.Empty<Income>(), default);
}
