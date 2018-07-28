namespace TWBuildingAssistant.Model.Buildings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class BuildingLevel
    {
        public BuildingBranch ContainingBranch { get; }

        public string Name { get; }

        public int Level { get; }

        public int Fertility { get; }

        public int PublicOrder { get; }

        public int Growth { get; }

        public int ResearchRate { get; }

        public int RegionalSanitation { get; }

        public int ProvincionalSanitation { get; }

        public int ReligiousInfluence { get; }

        public int ReligiousOsmosis { get; }

        public Religions.IReligion Religion
        {
            get { return ContainingBranch.Religion; }
        }

        public List<Effects.WealthBonus> Bonuses
        {
            get { return _bonuses.ToList(); }
        }

        public int Food(int fertility)
        {
            return RegularFood + fertility * FoodPerFertility;
        }

        public override string ToString()
        {
            return Name + " " + Level.ToString();
        }

        private Technologies.TechnologyLevel LevelOfTechnology { get; }

        private int RegularFood { get; }

        private int FoodPerFertility { get; }

        private Effects.WealthBonus[] _bonuses;

        public BuildingLevel(BuildingBranch branch, XElement element, ITechnologyLevelAssigner technologyLevelAssigner)
        {
            ContainingBranch = branch;
            Name = (string)element.Attribute("n");
            Level = (int)element.Attribute("l");
            if (element.Attribute("il") != null)
                LevelOfTechnology = technologyLevelAssigner.GetLevel((int)element.Attribute("t"), (bool)element.Attribute("il"));
            else
                LevelOfTechnology = technologyLevelAssigner.GetLevel((int)element.Attribute("t"), null);
            //
            if (element.Attribute("f") != null)
                RegularFood = (int)element.Attribute("f");
            if (element.Attribute("i") != null)
                Fertility = (int)element.Attribute("i");
            if (element.Attribute("fpf") != null)
                FoodPerFertility = (int)element.Attribute("fpf");
            if (element.Attribute("po") != null)
                PublicOrder = (int)element.Attribute("po");
            if (element.Attribute("g") != null)
                Growth = (int)element.Attribute("g");
            if (element.Attribute("rr") != null)
                ResearchRate = (int)element.Attribute("rr");
            if (element.Attribute("rs") != null)
                RegionalSanitation = (int)element.Attribute("rs");
            if (element.Attribute("ps") != null)
                ProvincionalSanitation = (int)element.Attribute("ps");
            if (element.Attribute("ri") != null)
                ReligiousInfluence = (int)element.Attribute("ri");
            if (element.Attribute("ro") != null)
                ReligiousOsmosis = (int)element.Attribute("ro");
            _bonuses = (from XElement subelement in element.Elements() select Effects.WealthBonusesFactory.MakeBonus(subelement)).ToArray();
        }

        public bool IsAvailable
        {
            get
            {
                return LevelOfTechnology.IsAvailable;
            }
        }
    }
}