namespace TWBuildingAssistant.Domain.OldModels;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Domain;

public class TechnologyTier
{
    public TechnologyTier(IEnumerable<BuildingLevel> universalLocks, IEnumerable<BuildingLevel> universalUnlocks, IEnumerable<BuildingLevel> antilegacyLocks, IEnumerable<BuildingLevel> antilegacyUnlocks)
    {
        this.UniversalLocks = universalLocks.ToImmutableArray();
        this.UniversalUnlocks = universalUnlocks.ToImmutableArray();
        this.AntilegacyLocks = antilegacyLocks.ToImmutableArray();
        this.AntilegacyUnlocks = antilegacyUnlocks.ToImmutableArray();
    }

    public ImmutableArray<BuildingLevel> UniversalLocks { get; set; }

    public ImmutableArray<BuildingLevel> UniversalUnlocks { get; set; }

    public ImmutableArray<BuildingLevel> AntilegacyLocks { get; set; }

    public ImmutableArray<BuildingLevel> AntilegacyUnlocks { get; set; }
}