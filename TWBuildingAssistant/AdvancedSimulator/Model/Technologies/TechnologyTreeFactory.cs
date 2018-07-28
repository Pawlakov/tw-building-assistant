namespace TWBuildingAssistant.Model.Technologies
{
    using System.Xml.Linq;

    public static class TechnologyTreeFactory
    {
        public static ITechnologyTree MakeTechnologyTree(string sourceFilename)
        {
            XDocument inputDocument = XDocument.Load("Technologies\\" + sourceFilename);
            XElement treeNode = inputDocument.Root;
            if ((string)treeNode.Attribute("t") == "expanded")
                return new ExpandedTechnologyTree(treeNode);
            return new SimpleTechnologyTree(treeNode);
        }
    }
}
