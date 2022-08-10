namespace TWBuildingAssistant.Domain.OldModels;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EnumsNET;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;
using TWBuildingAssistant.Domain.StateModels;

public class Faction
{
    private readonly TechnologyTier[] technologyTiers;

    private readonly BuildingBranch[] buildingBranches;

    public Faction(int id, string name, IEnumerable<TechnologyTier> technologyTiers, IEnumerable<BuildingBranch> buildingBranches)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Faction without name.");
        }

        if (technologyTiers == null)
        {
            throw new DomainRuleViolationException("Missing tech tiers.");
        }

        if (technologyTiers.Count() != 5)
        {
            throw new DomainRuleViolationException("Invalid tech tiers count.");
        }

        this.Id = id;
        this.Name = name;
        this.technologyTiers = technologyTiers.ToArray();
        this.buildingBranches = buildingBranches.ToArray();
    }

    public int Id { get; set; }

    public string Name { get; }
}