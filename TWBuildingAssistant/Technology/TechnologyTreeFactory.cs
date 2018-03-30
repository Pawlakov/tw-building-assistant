using System.Xml.Linq;
namespace Technologies
{
	public static class TechnologyTreeFactory
	{
		public static ITechnologyTree MakeTechnologyTree(string sourceFilename)
		{
			XDocument inputDocument = XDocument.Load(sourceFilename);
			XElement treeNode = inputDocument.Root;
			if ((string)treeNode.Attribute("t") == "legacy")
				return new LegacyTechnologyTree(treeNode);
			return new SimpleTechnologyTree(treeNode);
		}
	}
}
