using System;
using System.Xml;
namespace TWAssistant
{
	namespace Attila
	{
		public class Map
		{
			readonly ProvinceData[] provinces;
			//
			public Map(string filename)
			{
				XmlDocument sourceDocument = new XmlDocument();
				sourceDocument.Load(filename);
				XmlNodeList provinceNodeList = sourceDocument.GetElementsByTagName("province");
				provinces = new ProvinceData[provinceNodeList.Count];
				for (int whichProvince = 0; whichProvince < provinces.Length; ++whichProvince)
				{
					provinces[whichProvince] = new ProvinceData(provinceNodeList[whichProvince]);
				}
			}
			//
			public ProvinceData this[uint whichProvince]
			{
				get { return provinces[whichProvince]; }
			}
			//
			public void ShowList()
			{
				for (uint whichProvince = 0; whichProvince < provinces.Length; ++whichProvince)
				{
					Console.WriteLine("{0}. {1}", whichProvince, provinces[whichProvince].Name);
				}
			}
		}
	}
}
