namespace TWBuildingAssistant.Model.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TWBuildingAssistant.Model.Religions;

    public class InfluenceCalculator
    {
        private IEnumerable<IInfluence> influences;

        public InfluenceCalculator(Map.ProvinceTraditions traditions)
        {
            this.influences = traditions.Influences;
        }

        public void AddInfluence(IInfluence influence)
        {
            this.influences = this.influences.Append(influence);
        }

        public void AddInfluences(IEnumerable<IInfluence> influences)
        {
            this.influences = this.influences.Concat(influences);
        }

        public int PublicOrder()
        {
            var percentage = this.StateReligionPercentage();
            return (int)Math.Floor(-6 + (((percentage * 17) + 220) / 300));
        }

        public double StateReligionPercentage()
        {
            var state = 0;
            var other = 0;
            foreach (var influence in this.influences)
            {
                if (influence.Religion == null || influence.Religion.IsState)
                {
                    state += influence.Value;
                }
                else
                {
                    other += influence.Value;
                }
            }

            return 100 * (state / (other + (double)state));
        }
    }
}
