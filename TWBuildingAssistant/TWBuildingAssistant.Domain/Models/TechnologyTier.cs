﻿namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Domain;

public class TechnologyTier
{
    public TechnologyTier(IEnumerable<Effect> universalEffects, IEnumerable<Effect> antilegacyEffects, IEnumerable<Income> universalIncomes, IEnumerable<Income> antilegacyIncomes, IEnumerable<Influence> universalInfluences, IEnumerable<Influence> antilegacyInfluences, IEnumerable<BuildingLevel> universalLocks, IEnumerable<BuildingLevel> universalUnlocks, IEnumerable<BuildingLevel> antilegacyLocks, IEnumerable<BuildingLevel> antilegacyUnlocks)
    {
        this.UniversalEffects = universalEffects.ToImmutableArray();
        this.AntilegacyEffects = antilegacyEffects.ToImmutableArray();
        this.UniversalIncomes = universalIncomes.ToImmutableArray();
        this.AntilegacyIncomes = antilegacyIncomes.ToImmutableArray();
        this.UniversalInfluences = universalInfluences.ToImmutableArray();
        this.AntilegacyInfluences = antilegacyInfluences.ToImmutableArray();
        this.UniversalLocks = universalLocks.ToImmutableArray();
        this.UniversalUnlocks = universalUnlocks.ToImmutableArray();
        this.AntilegacyLocks = antilegacyLocks.ToImmutableArray();
        this.AntilegacyUnlocks = antilegacyUnlocks.ToImmutableArray();
    }

    public ImmutableArray<Effect> UniversalEffects { get; }

    public ImmutableArray<Effect> AntilegacyEffects { get; }

    public ImmutableArray<Income> UniversalIncomes { get; }

    public ImmutableArray<Income> AntilegacyIncomes { get; }

    public ImmutableArray<Influence> UniversalInfluences { get; }

    public ImmutableArray<Influence> AntilegacyInfluences { get; }

    public ImmutableArray<BuildingLevel> UniversalLocks { get; set; }

    public ImmutableArray<BuildingLevel> UniversalUnlocks { get; set; }

    public ImmutableArray<BuildingLevel> AntilegacyLocks { get; set; }

    public ImmutableArray<BuildingLevel> AntilegacyUnlocks { get; set; }
}