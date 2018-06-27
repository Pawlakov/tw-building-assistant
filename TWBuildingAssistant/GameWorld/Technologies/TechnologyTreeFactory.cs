using System.Xml.Linq;
namespace GameWorld.Technologies
{
	// Fabryka drzew technologii.
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
