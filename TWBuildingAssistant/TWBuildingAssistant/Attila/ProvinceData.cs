using System;
using System.Xml;
namespace TWAssistant
{
	namespace Attila
	{
		public class ProvinceData
		{
			readonly string name;
			readonly int fertility; //0-5 or 1-6 depending on mod
			readonly Climate climate;
			//
			readonly ProvinceTraditions traditions;
			//
			readonly RegionData[] regions;
			//
			public ProvinceData(XmlNode provinceNode)
			{
				name = provinceNode.Attributes.GetNamedItem("n").InnerText;
				try
				{
					fertility = Convert.ToInt32(provinceNode.Attributes.GetNamedItem("f").InnerText);
					if (fertility > 6 || fertility < 1)
						throw (new Exception("Fertility should be between 1 and 6 (is " + fertility + ")!"));
					climate = (Climate)Enum.Parse(typeof(Climate), provinceNode.Attributes.GetNamedItem("c").InnerText);
					//
					XmlNodeList childNodeList = provinceNode.ChildNodes;
					if (childNodeList.Count != 4)
						Console.WriteLine("Warning: Unexpected childnodes count ({0}) in province!", childNodeList.Count);
					traditions = new ProvinceTraditions(childNodeList[0]);
					//
					regions = new RegionData[childNodeList.Count - 1];
					for (int whichRegion = 0; whichRegion < regions.Length; ++whichRegion)
						regions[whichRegion] = new RegionData(childNodeList.Item(whichRegion + 1), !Convert.ToBoolean(whichRegion));
				}
				catch (Exception exception)
				{
					Console.WriteLine("Province {0} fell off a bike.", name);
					Console.WriteLine(exception.Message);
					Console.ReadKey();
				}
			}
			//
			public string Name
			{
				get { return name; }
			}
			public int Fertility
			{
				get { return fertility; }
			}
			public Climate Climate
			{
				get { return climate; }
			}
			public ProvinceTraditions Traditions
			{
				get { return traditions; }
			}
			public RegionData this[int whichRegion]
			{
				get { return regions[whichRegion]; }
			}
			public int RegionCount
			{
				get { return regions.Length; }
			}
		}
	}
}