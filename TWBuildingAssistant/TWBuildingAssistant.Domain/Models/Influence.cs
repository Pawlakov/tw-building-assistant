namespace TWBuildingAssistant.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Domain.Exceptions;

public struct Influence
{
    private readonly IEnumerable<KeyValuePair<int, int>> records;

    private readonly int state;

    public Influence(int? religionId, int value)
    {
        if (value <= 0)
        {
            throw new DomainRuleViolationException("Nonpositive influence.");
        }

        if (religionId != null)
        {
            this.records = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(religionId.Value, value) };
            this.state = 0;
        }
        else
        {
            this.records = new List<KeyValuePair<int, int>>();
            this.state = value;
        }
    }

    private Influence(IEnumerable<KeyValuePair<int, int>> records, int state)
    {
        this.state = state;
        if (records != null)
        {
            this.records = records;
        }
        else
        {
            this.records = new Dictionary<int, int>();
        }
    }

    public static Influence operator +(Influence left, Influence right)
    {
        var records = new List<KeyValuePair<int, int>>();
        if (left.records != null)
        {
            records.AddRange(left.records);
        }

        if (right.records != null)
        {
            foreach (var record in right.records)
            {
                var presentRecordIndex = records.FindIndex(x => x.Key == record.Key);
                if (presentRecordIndex > -1)
                {
                    records[presentRecordIndex] = new KeyValuePair<int, int>(record.Key, records[presentRecordIndex].Value + record.Value);
                }
                else
                {
                    records.Add(new KeyValuePair<int, int>(record.Key, record.Value));
                }
            }
        }

        var state = left.state + right.state;
        return new Influence(records, state);
    }

    public int PublicOrder(int stateReligionId)
    {
        var percentage = this.Percentage(stateReligionId);
        var result = -(int)Math.Floor((750 - percentage * 7) * 0.01);
        return result;
    }

    public double Percentage(int stateReligionId)
    {
        var state = this.state;
        var all = this.state;
        if (this.records != null)
        {
            foreach (var record in this.records)
            {
                all += record.Value;
                if (record.Key == stateReligionId || record.Key == null)
                {
                    state += record.Value;
                }
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