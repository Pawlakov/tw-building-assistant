namespace TWBuildingAssistant.Model.Technologies
{
    using System.Xml.Linq;

    using TWBuildingAssistant.Model.Religions;

    public static class TechnologyTreeFactory
    {
        public static ITechnologyTree MakeTechnologyTree(string sourceFilename, IParser<IReligion> religionParser)
        {
            XDocument inputDocument = XDocument.Load("Model\\Technologies\\" + sourceFilename);
            XElement treeNode = inputDocument.Root;
            ITechnologyTree result;
            if ((string)treeNode.Attribute("t") == "expanded")
                result = new ExpandedTechnologyTree(treeNode, religionParser);
            else
                result = new SimpleTechnologyTree(treeNode, religionParser);
            return result;
        }
    }
}
