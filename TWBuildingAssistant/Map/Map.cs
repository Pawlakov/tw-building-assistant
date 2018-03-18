using System;
using System.Xml;
using System.Globalization;
namespace Map
{
	/// <summary>
	/// Zawiera i udostępnia informacje o wszystkich prowincjach w grze.
	/// </summary>
	public class Map
	{
		/// <summary>
		/// Jedna jedyna instancja mapy.
		/// </summary>
		public static Map Singleton { get; private set; } = null;
		/// <summary>
		/// Tworzy instancję mapy dostępnej później poprzez właściwość TheMap. Nie wywoływać więcej niż raz.
		/// </summary>
		/// <returns>Nową TheMap.</returns>
		public static Map CreateSingleton()
		{
			if(Singleton == null)
			{
				Singleton = new Map();
				return Singleton;
			}
			throw new InvalidOperationException("Attempted to create second instance of a map (there can be only one).");
		}
		//
		//
		//
		private const string _fileName = "twa_map.xml";
		private readonly ProvinceData[] _provinces;
		private readonly XmlNodeList _nodes;
		private readonly string[] _names;
		private Map()
		{
			XmlDocument sourceDocument = new XmlDocument();
			try
			{
				sourceDocument.Load(_fileName);
			}
			catch(Exception exception)
			{
				throw new Exception(String.Format(CultureInfo.CurrentCulture, "Falied to load file {0}.", _fileName), exception);
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
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not read name of province {0}.", whichProvince), exception);
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
				if (whichProvince < 0 || whichProvince >= ProvincesCount)
					throw new ArgumentOutOfRangeException("whichProvince", whichProvince, "Index out of range.");
				if (_provinces[whichProvince] == null)
				{
					try
					{
						_provinces[whichProvince] = new ProvinceData(_nodes[whichProvince]);
					}
					catch(Exception exception)
					{
						throw new FormatException(String.Format("Failed to create province {0}.", _names[whichProvince]), exception);
					}
				}
				return _provinces[whichProvince];
			}
		}
		public int ProvincesCount
		{
			get { return _provinces.Length; }
		}
		public override string ToString()
		{
			string result = "Map\n";
			for (int whichProvince = 0; whichProvince < _provinces.Length; ++whichProvince)
				result += String.Format("{0}. {1}\n", whichProvince, _names[whichProvince]);
			return result;
		}
	}
}