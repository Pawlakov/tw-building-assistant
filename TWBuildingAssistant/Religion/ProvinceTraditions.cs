using System;
using System.Xml;
namespace Religion
{
	public class ProvinceTraditions
	{
		readonly int[] _traditions;
		public ProvinceTraditions(XmlNode traditionsNode)
		{
			_traditions = new int[Religion.ReligionTypesCount];
			//
			XmlNodeList nodeList = traditionsNode.ChildNodes;
			for (int whichTradition = 0; whichTradition < nodeList.Count; ++whichTradition)
			{
				Religion religion = (Religion)Enum.Parse(typeof(Religion), nodeList[whichTradition].Attributes.GetNamedItem("r").InnerText);
				_traditions[(int)religion] += Convert.ToInt32(nodeList[whichTradition].InnerText);
			}
		}
		public int GetTraditionExactly(Religion religion)
		{
			return _traditions[(int)religion];
		}
		public int GetTraditionExcept(Religion religion)
		{
			int result = 0;
			for (int whichReligion = 0; whichReligion < Globals.ReligionTypesCount; ++whichReligion)
				if (whichReligion != (int)religion)
					result += _traditions[whichReligion];
			return result;
		}
	}
}