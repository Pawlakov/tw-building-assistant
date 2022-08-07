namespace TWBuildingAssistant.Domain.Models;

using TWBuildingAssistant.Domain.Exceptions;

public class BuildingLevel
{
    public BuildingLevel(string name, Effect effect, Influence influence)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Building level without name.");
        }

        this.Name = name;
        this.Effect = effect;
        this.Influence = influence;
    }

    public string Name { get; }

    public Effect Effect { get; }

    public Influence Influence { get; }

    public static BuildingLevel Empty { get; } = new BuildingLevel("Empty", default, default);
}
