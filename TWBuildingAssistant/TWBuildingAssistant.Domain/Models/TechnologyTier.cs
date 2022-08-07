namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using System.Linq;

public class TechnologyTier
{
    public TechnologyTier(Effect universalEffect, Effect antilegacyEffect, Influence universalInfluence, Influence antilegacyInfluence, IEnumerable<BuildingLevel> universalLocks, IEnumerable<BuildingLevel> universalUnlocks, IEnumerable<BuildingLevel> antilegacyLocks, IEnumerable<BuildingLevel> antilegacyUnlocks)
    {
        this.UniversalEffect = universalEffect;
        this.AntilegacyEffect = antilegacyEffect;
        this.UniversalInfluence = universalInfluence;
        this.AntilegacyInfluence = antilegacyInfluence;
        this.UniversalLocks = universalLocks.ToList();
        this.UniversalUnlocks = universalUnlocks.ToList();
        this.AntilegacyLocks = antilegacyLocks.ToList();
        this.AntilegacyUnlocks = antilegacyUnlocks.ToList();
    }

    public Effect UniversalEffect { get; }

    public Effect AntilegacyEffect { get; }

    public Influence UniversalInfluence { get; }

    public Influence AntilegacyInfluence { get; }

    public IEnumerable<BuildingLevel> UniversalLocks { get; set; }

    public IEnumerable<BuildingLevel> UniversalUnlocks { get; set; }

    public IEnumerable<BuildingLevel> AntilegacyLocks { get; set; }

    public IEnumerable<BuildingLevel> AntilegacyUnlocks { get; set; }
}