using System;
using System.Xml;
using System.Globalization;
namespace Map
{
	/// <summary>
	/// Zbiór tradycji religijnych prowincji.
	/// </summary>
	public class ProvinceTraditions
	{
		private readonly int[] _traditions;
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
			_traditions = new int[Religions.ReligionsManager.Singleton.ReligionTypesCount];
			XmlNodeList nodeList = node.ChildNodes;
			for (int whichTradition = 0; whichTradition < nodeList.Count; ++whichTradition)
			{
				if(nodeList[whichTradition].Name != "tradition")
					throw new FormatException("Given node is not tradition node.");
				try
				{
					string innerText = nodeList[whichTradition].Attributes.GetNamedItem("r").InnerText;
					Religions.ReligionData religion = Religions.ReligionsManager.Singleton.Parse(innerText);
					if (religion == null)
						throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Value {0} was not recognized as any religion.", innerText));
					_traditions[Religions.ReligionsManager.Singleton.GetIndex(religion)] += Convert.ToInt32(nodeList[whichTradition].InnerText);
				}
				catch(Exception exception)
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
		public int GetTraditionExactly(Religions.ReligionData religion)
		{
			return _traditions[Religions.ReligionsManager.Singleton.GetIndex(religion)];
		}
		/// <summary>
		/// Zwraca wpływ wszystkich religii poza daną w tej prowincji.
		/// </summary>
		/// <param name="religion">Dana religia.</param>
		/// <returns>Wpływ dla wszystkich religii poza podaną.</returns>
		public int GetTraditionExcept(Religions.ReligionData religion)
		{
			int result = 0;
			for (int whichReligion = 0; whichReligion < Religions.ReligionsManager.Singleton.ReligionTypesCount; ++whichReligion)
				if (whichReligion != Religions.ReligionsManager.Singleton.GetIndex(religion))
					result += _traditions[whichReligion];
			return result;
		}
	}
}