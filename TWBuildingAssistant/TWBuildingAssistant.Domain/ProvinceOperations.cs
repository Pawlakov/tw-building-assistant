namespace TWBuildingAssistant.Domain;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Domain.Exceptions;

public static class ProvinceOperations
{
    public static Province Create(int id, string name, IEnumerable<Region> regions)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Province without name.");
        }

        if (regions == null)
        {
            throw new DomainRuleViolationException("No regions given for a province.");
        }

        if (regions.Count() != 3)
        {
            throw new DomainRuleViolationException("Invalid region count.");
        }

        return new Province(id, name, regions.ToImmutableArray());
    }
}