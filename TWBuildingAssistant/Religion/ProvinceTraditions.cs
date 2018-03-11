using System;
using System.Xml;
namespace Religion
{
	/// <summary>
	/// Zbiór tradycji religijnych prowincji.
	/// </summary>
	public class ProvinceTraditions
	{
		readonly int[] _traditions;
		/// <summary>
		/// Tworzy zbiór tradycji religijnych dla prowincji.
		/// </summary>
		/// <param name="node">Węzeł XML zawierający niezbędne informacje.</param>
		public ProvinceTraditions(XmlNode node)
		{
			if (node == null)
				throw new ReligionException("Próbowano utworzyć zbiór tradycji na podstawie pustej wartości.");
			_traditions = new int[Religion.ReligionTypesCount];
			XmlNodeList nodeList = node.ChildNodes;
			for (int whichTradition = 0; whichTradition < nodeList.Count; ++whichTradition)
			{
				string innerText = nodeList[whichTradition].Attributes.GetNamedItem("r").InnerText;
				if (innerText == null)
					throw new ReligionException("Wpis w zbiorze tradycji nie posiada parametru r.");
				Religion religion = Religion.Parse(innerText);
				if (religion == null)
					throw new ReligionException(String.Format("Wartość {0} nie została rozpoznana jako prawidłowa religia.", innerText));
				try
				{
					_traditions[(int)religion] += Convert.ToInt32(nodeList[whichTradition].InnerText);
				}
				catch(Exception exception)
				{
					throw new ReligionException(String.Format("Wartość wpisu w zbiorze tradycji nie jest prawidłowa (religia: {0})", innerText), exception);
				}
			}
		}
		/// <summary>
		/// Zwraca wpływ danej religii w tej prowincji.
		/// </summary>
		/// <param name="religion">Dana religia.</param>
		/// <returns>Wpływ dla podanej religii.</returns>
		public int GetTraditionExactly(Religion religion)
		{
			return _traditions[(int)religion];
		}
		/// <summary>
		/// Zwraca wpływ wszystkich religii poza daną w tej prowincji.
		/// </summary>
		/// <param name="religion">Dana religia.</param>
		/// <returns>Wpływ dla wszystkich religii poza podaną.</returns>
		public int GetTraditionExcept(Religion religion)
		{
			int result = 0;
			for (int whichReligion = 0; whichReligion < Religion.ReligionTypesCount; ++whichReligion)
				if (whichReligion != (int)religion)
					result += _traditions[whichReligion];
			return result;
		}
	}
}