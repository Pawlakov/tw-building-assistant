using System;
using System.Xml;
using System.Globalization;
namespace Religions
{
	/// <summary>
	/// Zbiór tradycji religijnych prowincji.
	/// </summary>
	public class ProvinceTraditions
	{
		// Stan wewnętrzny:
		//
		private readonly int[] _traditions;
		//
		// Interfejs zewnętrzny:
		//
		/// <summary>
		/// Tworzy zbiór tradycji religijnych dla prowincji.
		/// </summary>
		/// <param name="node">Węzeł XML zawierający niezbędne informacje.</param>
		public ProvinceTraditions(XmlNode node)
		{
			if (node == null)
				throw new ArgumentNullException("node");
			if (node.Name != "traditions")
				throw new ArgumentException("Given node is not traditions node.", "node");
			_traditions = new int[ReligionsManager.Singleton.ReligionTypesCount];
			XmlNodeList nodeList = node.ChildNodes;
			for (int whichTradition = 0; whichTradition < nodeList.Count; ++whichTradition)
			{
				if (nodeList[whichTradition].Name != "tradition")
					throw new FormatException("Given node is not tradition node.");
				try
				{
					string innerText = nodeList[whichTradition].Attributes.GetNamedItem("r").InnerText;
					IReligion religion = ReligionsManager.Singleton.Parse(innerText);
					if (religion == null)
						throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Value {0} was not recognized as any religion.", innerText));
					_traditions[ReligionsManager.Singleton.GetIndex(religion)] += Convert.ToInt32(nodeList[whichTradition].InnerText, CultureInfo.InvariantCulture);
				}
				catch (Exception exception)
				{
					throw new FormatException("Could not read tradition node's religion attribute.", exception);
				}
			}
		}
		/// <summary>
		/// Zwraca wpływ danej religii w tej prowincji.
		/// </summary>
		/// <param name="religion">Dana religia.</param>
		/// <returns>Wpływ dla podanej religii.</returns>
		public int GetTraditionExactly(IReligion religion)
		{
			if (religion == null)
				throw new ArgumentNullException("religion");
			return _traditions[ReligionsManager.Singleton.GetIndex(religion)];
		}
		/// <summary>
		/// Zwraca wpływ wszystkich religii poza daną w tej prowincji.
		/// </summary>
		/// <param name="religion">Dana religia.</param>
		/// <returns>Wpływ dla wszystkich religii poza podaną.</returns>
		public int GetTraditionExcept(IReligion religion)
		{
			if (religion == null)
				throw new ArgumentNullException("religion");
			int result = 0;
			for (int whichReligion = 0; whichReligion < ReligionsManager.Singleton.ReligionTypesCount; ++whichReligion)
				if (whichReligion != ReligionsManager.Singleton.GetIndex(religion))
					result += _traditions[whichReligion];
			return result;
		}
	}
}