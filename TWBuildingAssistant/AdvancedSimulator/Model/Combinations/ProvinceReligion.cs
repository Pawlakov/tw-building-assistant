namespace TWBuildingAssistant.Model.Combinations
{
    using System;

    public class ProvinceReligion
    {
        public ProvinceReligion(Map.ProvinceTraditions traditions)
        {
            Influence = traditions.StateReligionTradition();
            Counterinfluence = traditions.OtherReligionsTradition();
        }

        public void AddInfluence(int ammount, Religions.IReligion religion)
        {
            if (religion.IsState)
                Influence += ammount;
            else
                Counterinfluence += ammount;
        }

        public void AddInfluence(int ammount)
        {
            Influence += ammount;
        }

        public int PublicOrder
        {
            get
            {
                // Przybliżenie funkcji używanej przez grę.
                float percentage = StateReligionPercentage;
                return (int)Math.Floor(-6 + (percentage * 17 + 220) / 300);
            }
        }

        public int Influence { get; private set; }

        public int Counterinfluence { get; private set; }

        public float StateReligionPercentage
        {
            get
            {
                return 100 * (Influence / (Counterinfluence + (float)Influence));
            }
        }
    }
}
