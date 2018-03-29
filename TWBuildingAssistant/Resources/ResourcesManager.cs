using System.Linq;
using System.Xml.Linq;
namespace Resources
{
	/// <summary>
	/// Obiekt tej klasy przechowuje informacje na temat wszystkich zasobów dostępnych w grze.
	/// </summary>
	public class ResourcesManager
	{
		// Singleton:
		//
		private static ResourcesManager HiddenSingleton { get; set; }
		/// <summary>
		/// Odwołanie do jedynego obiektu.
		/// </summary>
		public static ResourcesManager Singleton
		{
			get
			{
				if (HiddenSingleton == null)
					HiddenSingleton = new ResourcesManager();
				return HiddenSingleton;
			}
		}
		private ResourcesManager()
		{
			XDocument sourceFile = XDocument.Load(_sourceFilename);
			_resources = (from XElement element in sourceFile.Root.Elements() select new Resource(element)).ToArray();
		}
		//
		// Stałe:
		//
		private const string _sourceFilename = "twa_resources.xml";
		//
		// Stan wewnętrzny:
		//
		private readonly Resource[] _resources;
		//
		// Interfejs publiczny:
		//
		/// <summary>
		/// Liczba dostępnych rodzajów zasobów.
		/// </summary>
		public int ResourceTypesCount
		{
			get { return _resources.Length; }
		}
		/// <summary>
		/// Sprawdza czy istnieje zasób o podanej nazwie. Jeśli tak to go zwraca. W przeciwnym wypadku zwraca null.
		/// </summary>
		/// <param name="input">Nazwa do spradzenia.</param>
		public Resource Parse(string input)
		{
			if (input == null)
				return null;
			foreach (Resource resource in _resources)
				if (resource.Name == input)
					return resource;
			return null;
		}
	}
}
