namespace TWBuildingAssistant.Domain;

using System;
using System.Collections.Generic;
using TWBuildingAssistant.Domain.Exceptions;

public static class InfluenceOperations
{
    public static Influence Create(in int? religionId, in int value)
    {
        return value switch
        {
            < 0 =>
                throw new DomainRuleViolationException("Negative influence."),
            _ =>
                new Influence(religionId, value),
        };
    }

    public static int Collect(IEnumerable<Influence> influences, in int stateReligionId)
    {
        var percentage = Percentage(influences, stateReligionId);
        var result = -(int)Math.Floor((750 - (percentage * 7)) * 0.01);
        return result;
    }

    private static double Percentage(IEnumerable<Influence> influences, in int stateReligionId)
    {
        var state = 0;
        var all = 0;
        foreach (var record in influences)
        {
            all += record.Value;
            if (record.ReligionId == stateReligionId || record.ReligionId == null)
            {
                state += record.Value;
            }
        }

        if (all == 0)
        {
            return 100;
        }

        var percentage = 100 * (state / (double)all);
        return percentage;
    }
}