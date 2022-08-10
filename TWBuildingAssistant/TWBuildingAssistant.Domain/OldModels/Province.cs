namespace TWBuildingAssistant.Domain.OldModels;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;

public class Province
{
    public Province(int id, string name, IEnumerable<Region> regions, int climateId, Effect baseEffect, IEnumerable<Income> baseIncomes, IEnumerable<Influence> baseInfluences)
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

        this.Id = id;
        this.Name = name;
        this.BaseEffect = baseEffect;
        this.BaseIncomes = baseIncomes.ToImmutableArray();
        this.BaseInfluences = baseInfluences.ToImmutableArray();
        this.Regions = regions.ToImmutableArray();
        this.ClimateId = climateId;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public int ClimateId { get; init; }

    public ImmutableArray<Region> Regions { get; init; }

    public Effect BaseEffect { get; init; }

    public ImmutableArray<Income> BaseIncomes { get; init; }

    public ImmutableArray<Influence> BaseInfluences { get; init; }
}