namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;

public class Religion
{
    public Religion(string name, Effect effectWhenState, IEnumerable<Income> incomesWhenState, Influence influenceWhenState)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Religion without name.");
        }

        this.Name = name;
        this.EffectWhenState = effectWhenState;
        this.IncomesWhenState = incomesWhenState;
        this.InfluenceWhenState = influenceWhenState;
    }

    public string Name { get; }

    public Effect EffectWhenState { get; }

    public IEnumerable<Income> IncomesWhenState { get; }

    public Influence InfluenceWhenState { get; }
}