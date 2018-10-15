namespace TWBuildingAssistant.Model.Effects
{
    using System;
    using System.Collections.Generic;

    public static class InfluenceCalculator
    {
        public static int PublicOrder(IEnumerable<IInfluence> influences)
        {
            return PublicOrder(Percentage(influences));
        }

        public static int PublicOrder(double percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percentage), percentage, "Not a valid state religion percentage.");
            }

            var result = -(int)Math.Floor((750 - (percentage * 7)) * 0.01);
            return result;
        }

        public static double Percentage(IEnumerable<IInfluence> influences)
        {
            if (influences == null)
            {
                throw new ArgumentNullException(nameof(influences));
            }

            var state = 0;
            var other = 0;
            foreach (var influence in influences)
            {
                try
                {
                    var religion = influence.GetReligion();
                    if (religion == null || religion.IsState)
                    {
                        state += influence.Value;
                    }
                    else
                    {
                        other += influence.Value;
                    }
                }
                catch (Exception e)
                {
                    throw new EffectsException("Failed to get influence's religion.", e);
                }
            }

            if (other + state == 0)
            {
                return 100;
            }

            var percentage = 100 * (state / (other + (double)state));
            return percentage;
        }
    }
}