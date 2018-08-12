namespace TWBuildingAssistant.Model.Factions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;

    public class FactionsManager
    {
        private const string FileName = "Model\\Factions\\twa_factions.xml";

        private readonly IReligionParser religionsParser;

        private readonly Map.IResourceParser resourcesParser;

        private readonly XElement[] elements;

        private readonly Faction[] factions;

        public FactionsManager(IReligionParser religionsParser, Map.IResourceParser resourcesParser)
        {
            var sourceFile = XDocument.Load(FileName);
            this.elements = (from XElement element in sourceFile.Root.Elements() select element).ToArray();
            this.factions = new Faction[this.elements.Length];
            this.religionsParser = religionsParser;
            this.resourcesParser = resourcesParser;
        }

        public int FactionsCount => this.elements.Length;

        public Faction Faction
        {
            get
            {
                if (this.factions[this.FactionIndex] == null)
                {
                    this.factions[this.FactionIndex] = new Faction(
                    this.elements[this.FactionIndex],
                    this.resourcesParser,
                    this.religionsParser);
                }

                return this.factions[this.FactionIndex];
            }
        }

        public Effects.IProvincionalEffect Effect => Faction.Effect;

        public IEnumerable<KeyValuePair<int, string>> AllFactionsNames
        {
            get
            {
                var result = new List<KeyValuePair<int, string>>(this.FactionsCount);
                for (var whichFaction = 0; whichFaction < this.FactionsCount; ++whichFaction)
                {
                    result.Add(
                    new KeyValuePair<int, string>(whichFaction, (string)this.elements[whichFaction].Attribute("n")));
                }

                return result;
            }
        }

        private int FactionIndex { get; set; } = -1;

        public void ChangeFaction(int whichFaction)
        {
            this.FactionIndex = whichFaction;
        }
    }
}