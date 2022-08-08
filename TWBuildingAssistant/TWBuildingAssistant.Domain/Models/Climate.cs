namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Domain;

public class Climate
{
    private readonly IDictionary<int, IDictionary<int, Effect>> effects;
    private readonly IDictionary<int, IDictionary<int, IEnumerable<Income>>> incomes;

    public Climate(IDictionary<int, IDictionary<int, Effect>> effects, IDictionary<int, IDictionary<int, IEnumerable<Income>>> incomes)
    {
        this.effects = effects.ToDictionary(x => x.Key, x => (IDictionary<int, Effect>)x.Value.ToDictionary(y => y.Key, y => y.Value));
        this.incomes = incomes.ToDictionary(x => x.Key, x => (IDictionary<int, IEnumerable<Income>>)x.Value.ToDictionary(y => y.Key, y => y.Value.ToList().AsEnumerable()));
    }

    public Effect GetEffect(int seasonId, int weatherId)
    {
        return this.effects[seasonId][weatherId];
    }

    public IEnumerable<Income> GetIncomes(int seasonId, int weatherId)
    {
        return this.incomes[seasonId][weatherId];
    }
}