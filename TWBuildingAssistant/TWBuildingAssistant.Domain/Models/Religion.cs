namespace TWBuildingAssistant.Domain.Models;

using TWBuildingAssistant.Domain.Exceptions;

public class Religion
{
    public Religion(string name, Effect effectWhenState = default, Influence influenceWhenState = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Religion without name.");
        }

        this.Name = name;
        this.EffectWhenState = effectWhenState;
        this.InfluenceWhenState = influenceWhenState;
    }

    public string Name { get; }

    public Effect EffectWhenState { get; }

    public Influence InfluenceWhenState { get; }
}