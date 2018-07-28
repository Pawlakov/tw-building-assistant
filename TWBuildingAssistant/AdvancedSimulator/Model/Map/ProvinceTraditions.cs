namespace TWBuildingAssistant.Model.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class ProvinceTraditions
    {
        private Religions.IReligion StateReligion { get; set; }

        private readonly Dictionary<Religions.IReligion, int> _traditions;

        public ProvinceTraditions(XElement element, IReligionParser religionsManager, IStateReligionTracker stateReligionTracker)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            if (religionsManager == null)
                throw new ArgumentNullException("religionsManager");
            if (!ValidateElement(element, religionsManager, out string message))
                throw new FormatException(message);
            _traditions = new Dictionary<Religions.IReligion, int>(element.Elements().Count());
            foreach (XElement traditionElement in element.Elements())
                _traditions[religionsManager.Parse((string)traditionElement.Attribute("r"))] = (int)traditionElement;
            StateReligion = stateReligionTracker.StateReligion;
            stateReligionTracker.StateReligionChanged += (IStateReligionTracker sender, EventArgs e) => { StateReligion = sender.StateReligion; };
        }

        public int StateReligionTradition()
        {
            if (_traditions.ContainsKey(StateReligion))
                return _traditions[StateReligion];
            return 0;
        }

        public int OtherReligionsTradition()
        {
            return _traditions.Sum((KeyValuePair<Religions.IReligion, int> pair) => { return pair.Value; }) - StateReligionTradition();
        }

        public static bool ValidateElement(XElement element, IReligionParser religionsManager, out string message)
        {
            foreach (XElement subelement in element.Elements())
            {
                if (subelement.Attribute("r") == null)
                {
                    message = "XML element lacks 'r' attribute in one of sub-elements.";
                    return false;
                }
                if (religionsManager.Parse((string)subelement.Attribute("r")) == null)
                {
                    message = "Attribute 'r' of XML element's sub-element is not recognized as a religion's name.";
                    return false;
                }
                if (!Int32.TryParse((string)subelement, out int dummy))
                {
                    message = "Content of XML element's sub-element is not recognized as an integer value.";
                    return false;
                }
            }
            message = "XML element is a valid representation of a province's traditions.";
            return true;
        }
    }
}
