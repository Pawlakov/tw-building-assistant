namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;

public class BuildingLevel
{
    public BuildingLevel(string name, Effect effect, IEnumerable<Income> incomes, IEnumerable<Influence> influences)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Building level without name.");
        }

        this.Name = name;
        this.Effect = effect;
        this.Incomes = incomes.ToList();
        this.Influences = influences.ToList();
    }

    public string Name { get; }

    public Effect Effect { get; }

    public IEnumerable<Income> Incomes { get; }

    public IEnumerable<Influence> Influences { get; }

    public static BuildingLevel Empty { get; } = new BuildingLevel("Empty", default, Enumerable.Empty<Income>(), Enumerable.Empty<Influence>());
}
