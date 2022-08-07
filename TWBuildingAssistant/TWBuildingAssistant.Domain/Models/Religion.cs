namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;

public class Religion
{
    public Religion(int id, string name, Effect effectWhenState, IEnumerable<Income> incomesWhenState, int stateInfluenceWhenState)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Religion without name.");
        }

        this.Id = id;
        this.Name = name;
        this.EffectWhenState = effectWhenState;
        this.IncomesWhenState = incomesWhenState;
        this.StateInfluenceWhenState = stateInfluenceWhenState;
    }

    public int Id { get; set; }

    public string Name { get; }

    public Effect EffectWhenState { get; }

    public IEnumerable<Income> IncomesWhenState { get; }

    public int StateInfluenceWhenState { get; }
}