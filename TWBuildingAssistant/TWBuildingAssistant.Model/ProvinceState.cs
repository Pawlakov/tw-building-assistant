namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public struct ProvinceState
    {
        public ProvinceState(IEnumerable<int> sanitation, int food, int publicOrder, int religiousOsmosis, int researchRate, int growth, double wealth)
        {
            this.Sanitation = sanitation.ToList();
            this.Food = food;
            this.PublicOrder = publicOrder;
            this.ReligiousOsmosis = religiousOsmosis;
            this.ResearchRate = researchRate;
            this.Growth = growth;
            this.Wealth = wealth;
        }

        public IEnumerable<int> Sanitation { get; }

        public int Food { get; }

        public int PublicOrder { get; }

        public int ReligiousOsmosis { get; }

        public int ResearchRate { get; }

        public int Growth { get; }

        public double Wealth { get; }
    }
}