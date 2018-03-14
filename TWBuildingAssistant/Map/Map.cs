using System;
using System.Xml;
namespace Map
{
	public enum Resource { NONE, IRON, LEAD, GEMSTONES, OLIVE, FUR, WINE, SILK, MARBLE, SALT, GOLD, DYE, LUMBER, SPICE };
	/// <summary>
	/// Zawiera i udostępnia informacje o wszystkich prowincjach w grze.
	/// </summary>
	public class Map
	{
		private static Map _map = null;
		/// <summary>
		/// Jedna jedyna instancja mapy.
		/// </summary>
		public static Map TheMap
		{
			get
			{
				if (_map == null)
					_map = new Map();
				return _map;
			}
		}
		//
		//
		//
		const string _fileName = "twa_map.xml";
		readonly ProvinceData[] _provinces;
		readonly XmlNodeList _nodes;
		readonly string[] _names;
		Map()
		{
			XmlDocument sourceDocument = new XmlDocument();
			try
			{
				sourceDocument.Load(_fileName);
			}
			catch(Exception exception)
			{
				throw new MapException("Nie udało się otworzyć pliku mapy.", exception);
			}
			_nodes = sourceDocument.GetElementsByTagName("province");
			_provinces = new ProvinceData[_nodes.Count];
			_names = new string[_nodes.Count];
			for (int whichProvince = 0; whichProvince < _provinces.Length; ++whichProvince)
			{
				try
				{
					_names[whichProvince] = _nodes[whichProvince].Attributes.GetNamedItem("n").InnerText;
				}
				catch(Exception exception)
				{
					throw new MapException(String.Format("Nie udało się odczytać nazwy prowincji o indeksie {0}.", whichProvince), exception);
				}
			}
		}
		/// <summary>
		/// Indekser udostępniający poszczególne prowincje na mapie.
		/// </summary>
		/// <param name="whichProvince">Indeks żądanej prowincji.</param>
		/// <returns>Dane prowincji po podanym indeksem.</returns>
		public ProvinceData this[int whichProvince]
		{
			get
			{
				if (_provinces[whichProvince] == null)
				{
					try
					{
						_provinces[whichProvince] = new ProvinceData(_nodes[whichProvince]);
					}
					catch(Exception exception)
					{
						throw new MapException(String.Format("Nie powiodło się utworzenie informacji o prowincji {0} (indeks: {1}).", _names[whichProvince], whichProvince), exception);
					}
				}
				return _provinces[whichProvince];
			}
		}
		public override string ToString()
		{
			string result = "Map\n";
			for (int whichProvince = 0; whichProvince < _provinces.Length; ++whichProvince)
				result += String.Format("{0}. {1}\n", whichProvince, _names[whichProvince]);
			return result;
		}
	}
	public class MapException : Exception
	{
		public MapException() : base("Błąd w module mapy.") { }
		public MapException(string message) : base(message) { }
		public MapException(string message, Exception innerException) : base(message, innerException) { }
	}
}