namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Domain.Exceptions;

public class Season
{
    public Season(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Season without name.");
        }

        this.Name = name;
    }

    public string Name { get; }
}