namespace TWBuildingAssistant.Model.Factions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class FactionsManager
    {
        readonly XElement[] _elements;

        readonly Faction[] _factions;

        public FactionsManager(Map.IReligionParser religionsParser, Map.IResourceParser resourcesParser)
        {
            XDocument sourceFile = XDocument.Load(_fileName);
            _elements = (from XElement element in sourceFile.Root.Elements() select element).ToArray();
            _factions = new Faction[_elements.Length];
            _religionsParser = religionsParser;
            _resourcesParser = resourcesParser;
        }

        private const string _fileName = "Model\\Factions\\twa_factions.xml";

        private readonly Map.IReligionParser _religionsParser;

        private readonly Map.IResourceParser _resourcesParser;

        private int FactionIndex { get; set; } = -1;

        public int FactionsCount
        {
            get { return _elements.Length; }
        }

        public void ChangeFaction(int whichFaction)
        {
            FactionIndex = whichFaction;
        }

        public Faction Faction
        {
            get
            {
                // Jeżeli obiekt nie został póki co stworzony to teraz powstanie.
                if (_factions[FactionIndex] == null)
                    _factions[FactionIndex] = new Faction(_elements[FactionIndex], _resourcesParser, _religionsParser);
                return _factions[FactionIndex];
            }
        }

        public Effects.IProvincionalEffect Effect => Faction.Effect;

        public IEnumerable<KeyValuePair<int, string>> AllFactionsNames
        {
            get
            {
                List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>(FactionsCount);
                for (int whichFaction = 0; whichFaction < FactionsCount; ++whichFaction)
                    result.Add(new KeyValuePair<int, string>(whichFaction, (string)_elements[whichFaction].Attribute("n")));
                return result;
            }
        }
    }
}