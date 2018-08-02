namespace TWBuildingAssistant.Model.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class ProvinceTraditions
    {
        private readonly IEnumerable<Effects.IInfluence> influences;

        public ProvinceTraditions(XElement element, IReligionParser religionParser)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (religionParser == null)
            {
                throw new ArgumentNullException(nameof(religionParser));
            }

            if (!ValidateElement(element, religionParser, out var message))
            {
                throw new FormatException(message);
            }

            this.influences = from XElement item in element.Elements()
                              select new Effects.Influence()
                                     {
                                     Religion = religionParser.Parse(
                                     (string)item.Attribute("r")),
                                     Value = (int)item
                                     };
        }

        public IEnumerable<Effects.IInfluence> Influences => this.influences.ToArray();

        public static bool ValidateElement(XElement element, IReligionParser religionParser, out string message)
        {
            foreach (var subelement in element.Elements())
            {
                if (subelement.Attribute("r") == null)
                {
                    message = "XML element lacks 'r' attribute in one of sub-elements.";
                    return false;
                }

                if (religionParser.Parse((string)subelement.Attribute("r")) == null)
                {
                    message = "Attribute 'r' of XML element's sub-element is not recognized as a religion's name.";
                    return false;
                }

                if (!int.TryParse((string)subelement, out _))
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
