namespace TWBuildingAssistant.Model.Technologies
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Effects;

    public class TechnologyLevel
    {
        private ITechnologyTree ContainingTree { get; }

        public Effects.IProvincialEffect Effect { get; private set; }

        public bool IsAvailable { get; private set; }

        public void Cumulate(TechnologyLevel added)
        {
            if (added == null)
            {
                throw new ArgumentNullException(nameof(added));
            }

            this.Effect = this.Effect.Aggregate(added.Effect);
        }

        public TechnologyLevel(ITechnologyTree containingTree, XElement element)
        {
            this.ContainingTree = containingTree;
            containingTree.DesiredTechnologyChanged += (ITechnologyTree sender, EventArgs e) => { IsAvailable = containingTree.IsLevelReasearched(this); };

            var sanitation = 0;
            var fertility = 0;
            var growth = 0;
            var publicOrder = 0;
            var religiousInfluence = 0;
            var religiousOsmosis = 0;
            var food = 0;
            var researchRate = 0;
            var temporary = element.Attribute("s");
            if (temporary != null)
                sanitation = (int)temporary;
            temporary = element.Attribute("i");
            if (temporary != null)
                fertility = (int)temporary;
            temporary = element.Attribute("g");
            if (temporary != null)
                growth = (int)temporary;
            temporary = element.Attribute("po");
            if (temporary != null)
                publicOrder = (int)temporary;
            temporary = element.Attribute("ri");
            if (temporary != null)
                religiousInfluence = (int)temporary;
            temporary = element.Attribute("ro");
            if (temporary != null)
                religiousOsmosis = (int)temporary;
            temporary = element.Attribute("f");
            if (temporary != null)
                food = (int)temporary;
            temporary = element.Attribute("rr");
            if (temporary != null)
                researchRate = (int)temporary;
            var bonuses = new IBonus[0];
            if (!string.IsNullOrEmpty((string)element))
                 bonuses = JsonConvert.DeserializeObject<Bonus[]>((string)element, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
            var influences = new IInfluence[0];
            if (religiousInfluence > 0)
                influences = new IInfluence[] { new Influence { ReligionId = null, Value = religiousInfluence } };

            this.Effect = new ProvincialEffect()
                          {
                          Bonuses = bonuses,
                          Fertility = fertility,
                          Growth = growth,
                          Influences = influences,
                          ProvincialSanitation = sanitation,
                          PublicOrder = publicOrder,
                          ReligiousOsmosis = religiousOsmosis,
                          RegularFood = food,
                          ResearchRate = researchRate
                          };
        }

        public TechnologyLevel(ITechnologyTree containingTree)
        {
            this.ContainingTree = containingTree;
            containingTree.DesiredTechnologyChanged += (sender, e) => { this.IsAvailable = containingTree.IsLevelReasearched(this); };
            this.Effect = new Effects.ProvincialEffect();
        }
    }
}
