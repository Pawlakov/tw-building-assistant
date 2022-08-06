namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Domain.Exceptions;

public class Religion
{
    public Religion(string name, Effect effectWhenState = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Religion without name.");
        }

        this.Name = name;
        this.EffectWhenState = effectWhenState;
    }

    public string Name { get; }

    public Effect EffectWhenState { get; }
}