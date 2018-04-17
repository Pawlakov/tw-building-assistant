using System;
using System.Xml;
namespace TWAssistant
{
	namespace Attila
	{
		public class ProvinceTraditions
		{
			readonly int[] traditions;
			public ProvinceTraditions(XmlNode traditionsNode)
			{
				traditions = new int[Globals.ReligionTypesCount];
				//
				XmlNodeList nodeList = traditionsNode.ChildNodes;
				for (int whichReligion = 0; whichReligion < Globals.ReligionTypesCount; ++whichReligion)
					traditions[whichReligion] = 0;
				for (int whichTradition = 0; whichTradition < nodeList.Count; ++whichTradition)
				{
					Religion religion = (Religion)Enum.Parse(typeof(Religion), nodeList[whichTradition].Attributes.GetNamedItem("r").InnerText);
					traditions[(int)religion] += Convert.ToInt32(nodeList[whichTradition].InnerText);
				}
			}
			public int GetTradionExactly(Religion religion)
			{
				return traditions[(int)religion];
			}
			public int GetTraditionExcept(Religion religion)
			{
				int result = 0;
				for (int whichReligion = 0; whichReligion < Globals.ReligionTypesCount; ++whichReligion)
					if (whichReligion != (int)religion)
						result += traditions[whichReligion];
				return result;
			}
		}
	}
}