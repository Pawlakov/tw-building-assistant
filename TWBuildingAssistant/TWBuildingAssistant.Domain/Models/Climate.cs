namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Domain;

public class Climate
{
    private readonly IDictionary<Season, IDictionary<Weather, Effect>> effects;
    private readonly IDictionary<Season, IDictionary<Weather, IEnumerable<Income>>> incomes;

    public Climate(IDictionary<Season, IDictionary<Weather, Effect>> effects, IDictionary<Season, IDictionary<Weather, IEnumerable<Income>>> incomes)
    {
        this.effects = effects.ToDictionary(x => x.Key, x => (IDictionary<Weather, Effect>)x.Value.ToDictionary(y => y.Key, y => y.Value));
        this.incomes = incomes.ToDictionary(x => x.Key, x => (IDictionary<Weather, IEnumerable<Income>>)x.Value.ToDictionary(y => y.Key, y => y.Value.ToList().AsEnumerable()));
    }

    public Effect GetEffect(Season season, Weather weather)
    {
        return this.effects[season][weather];
    }

    public IEnumerable<Income> GetIncomes(Season season, Weather weather)
    {
        return this.incomes[season][weather];
    }
}