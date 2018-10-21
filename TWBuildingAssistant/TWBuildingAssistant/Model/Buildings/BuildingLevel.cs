namespace TWBuildingAssistant.Model.Buildings
{
    using System.Xml.Linq;

    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Technologies;

    public class BuildingLevel
    {
        private readonly TechnologyLevel unlockingLevel;

        private readonly TechnologyLevel lockingLevel;

        public BuildingLevel(BuildingBranch branch, XElement element, ITechnologyLevelAssigner technologyLevelAssigner)
        {
            this.ContainingBranch = branch;
            this.Name = (string)element.Attribute("n");
            this.Level = (int)element.Attribute("l");
            if (element.Attribute("il") != null)
            {
                if ((bool)element.Attribute("il"))
                {
                    this.lockingLevel = technologyLevelAssigner.GetLevel(
                        (int)element.Attribute("t"),
                        (bool)element.Attribute("il"));
                }
                else
                {
                    this.unlockingLevel = technologyLevelAssigner.GetLevel(
                        (int)element.Attribute("t"),
                        (bool)element.Attribute("il"));
                }
            }
            else
            {
                this.unlockingLevel = technologyLevelAssigner.GetLevel((int)element.Attribute("t"), null);
            }

            var regularFood = 0;
            var fertility = 0;
            var foodPerFertility = 0;
            var publicOrder = 0;
            var growth = 0;
            var researchRate = 0;
            var regionalSanitation = 0;
            var provincialSanitation = 0;
            var religiousInfluence = 0;
            var religiousOsmosis = 0;
            if (element.Attribute("f") != null)
            {
                regularFood = (int)element.Attribute("f");
            }

            if (element.Attribute("i") != null)
            {
                fertility = (int)element.Attribute("i");
            }

            if (element.Attribute("fpf") != null)
            {
                foodPerFertility = (int)element.Attribute("fpf");
            }

            if (element.Attribute("po") != null)
            {
                publicOrder = (int)element.Attribute("po");
            }

            if (element.Attribute("g") != null)
            {
                growth = (int)element.Attribute("g");
            }

            if (element.Attribute("rr") != null)
            {
                researchRate = (int)element.Attribute("rr");
            }

            if (element.Attribute("rs") != null)
            {
                regionalSanitation = (int)element.Attribute("rs");
            }

            if (element.Attribute("ps") != null)
            {
                provincialSanitation = (int)element.Attribute("ps");
            }

            if (element.Attribute("ri") != null)
            {
                religiousInfluence = (int)element.Attribute("ri");
            }

            if (element.Attribute("ro") != null)
            {
                religiousOsmosis = (int)element.Attribute("ro");
            }

            var influences = new IInfluence[0];
            var bonuses = new IBonus[0];
            if (!string.IsNullOrEmpty((string)element))
            {
                bonuses = JsonConvert.DeserializeObject<Bonus[]>(
                    (string)element,
                    new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
            }

            if (religiousInfluence > 0)
            {
                influences = new IInfluence[]
                                 {
                                     new Influence()
                                         {
                                             ReligionId = this.ContainingBranch.Religion?.Id, Value = religiousInfluence
                                         }
                                 };
            }

            this.Effect = new RegionalEffect()
                          {
                          Bonuses = bonuses,
                          Fertility = fertility,
                          FertilityDependentFood = foodPerFertility,
                          Growth = growth,
                          Influences = influences,
                          ProvincialSanitation = provincialSanitation,
                          PublicOrder = publicOrder,
                          RegionalSanitation = regionalSanitation,
                          RegularFood = regularFood,
                          ReligiousOsmosis = religiousOsmosis,
                          ResearchRate = researchRate
                          };
        }

        public BuildingBranch ContainingBranch { get; }

        public string Name { get; }

        public int Level { get; }

        public IRegionalEffect Effect { get; }

        public bool IsAvailable
        {
            get
            {
                if (this.lockingLevel != null)
                {
                    return !this.lockingLevel.IsAvailable;
                }

                if (this.unlockingLevel != null)
                {
                    return this.unlockingLevel.IsAvailable;
                }

                return true;
            }
        }

        public override string ToString()
        {
            return $"{this.Name} {this.Level}";
        }
    }
}