namespace TWBuildingAssistant.Domain;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Domain.Exceptions;

public static class ReligionOperations
{
    public static Religion Create(in int id, in string name, in Effect effectWhenState, IEnumerable<Income> incomesWhenState, in int stateInfluenceWhenState)
    {
        return (id, name) switch
        {
            (0, _) =>
                throw new DomainRuleViolationException("Religion without id."),
            (_, null or "") =>
                throw new DomainRuleViolationException("Religion without name."),
            _ =>
                new Religion(id, name, effectWhenState, (incomesWhenState ?? Enumerable.Empty<Income>()).ToImmutableArray(), stateInfluenceWhenState),
        };
    }
}