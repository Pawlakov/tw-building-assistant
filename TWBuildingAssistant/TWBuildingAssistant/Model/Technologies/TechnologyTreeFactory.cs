namespace TWBuildingAssistant.Model.Technologies
{
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;

    public static class TechnologyTreeFactory
    {
        public static ITechnologyTree MakeTechnologyTree(XElement element, Parser<IReligion> religionParser)
        {
            ITechnologyTree result;
            if ((string)element.Attribute("t") == "expanded")
                result = new ExpandedTechnologyTree(element, religionParser);
            else
                result = new SimpleTechnologyTree(element, religionParser);
            return result;
        }
    }
}
