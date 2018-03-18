using System;
using System.Xml;
using System.Globalization;
namespace Resources
{
	/// <summary>
	/// Obiekt tej klasy zawiera informacje na temat jednego z zasobów dostępnych w grze.
	/// </summary>
	public class Resource
	{
		/// <summary>
		/// Odpowiadająca zasobowi gałąź budynków.
		/// </summary>
		public Buildings.BuildingBranch BuildingBranch { get; }
		/// <summary>
		/// Nazwa zasobu.
		/// </summary>
		public string Name { get; }
		internal Resource(XmlNode node)
		{
			if (node == null)
				throw new ArgumentNullException("node");
			if (node.Name != "resource")
				throw new ArgumentException("This XML node is not a resource description node.", "node");
			try
			{
				Name = node.Attributes.GetNamedItem("n").InnerText;
			}
			catch (Exception exception)
			{
				throw new FormatException("Could not read name attribute from XML node.", exception);
			}
			XmlNodeList children = node.ChildNodes;
			if (children.Count != 0)
				throw new FormatException("Every resource should have exactly one corresponding building branch.");
			try
			{
				BuildingBranch = Buildings.BuildingBranch.MakeBuildingBranch(children[0]);
			}
			catch (Exception exception)
			{
				throw new Exception(String.Format(CultureInfo.CurrentCulture, "Could not make a resource building branch for {0}.", Name), exception);
			}
		}
	}
}
