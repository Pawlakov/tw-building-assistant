namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public class Climate
    {
        private readonly IDictionary<Season, IDictionary<Weather, Effect>> effects;

        public Climate(IDictionary<Season, IDictionary<Weather, Effect>> effects)
        {
            this.effects = effects.ToDictionary(x => x.Key, x => (IDictionary<Weather, Effect>)x.Value.ToDictionary(y => y.Key, y => y.Value));
        }

        public Effect GetEffect(Season season, Weather weather)
        {
            return this.effects[season][weather];
        }
    }
}