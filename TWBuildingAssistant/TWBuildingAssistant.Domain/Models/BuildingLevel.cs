namespace TWBuildingAssistant.Domain.Models;

using System;
using System.Collections.Generic;
using System.Text;
using TWBuildingAssistant.Domain.Exceptions;

public class BuildingLevel
{
    public BuildingLevel(string name, Effect effect)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Building level without name.");
        }

        this.Name = name;
        this.Effect = effect;
    }

    public string Name { get; }

    public Effect Effect { get; set; }

    public static BuildingLevel Empty { get; } = new BuildingLevel("Empty", default);
}
