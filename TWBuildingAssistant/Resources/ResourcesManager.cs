using System;
using System.Xml;
using System.Globalization;
namespace Resources
{
	/// <summary>
	/// Obiekt tej klasy przechowuje informacje na temat wszystkich zasobów dostępnych w grze.
	/// </summary>
	public class ResourcesManager
	{
		/// <summary>
		/// Odwołanie do jedynego obiektu.
		/// </summary>
		public static ResourcesManager Singleton { get; private set; } = null;
		/// <summary>
		/// Tworzy jedyny obiekt a następnie go zwraca. Nie wywoływać więcej niż raz.
		/// </summary>
		public static ResourcesManager CreateSingleton()
		{
			if (Singleton == null)
			{
				Singleton = new ResourcesManager();
				return Singleton;
			}
			throw new InvalidOperationException("You cannot create another singleton when one already exists.");
		}
		//
		//
		//
		private const string _fileName = "twa_resources.xml";
		private readonly Resource[] _resources;
		private ResourcesManager()
		{
			XmlDocument sourceFile = new XmlDocument();
			try
			{
				sourceFile.Load(_fileName);
			}
			catch (Exception exception)
			{
				throw new Exception(String.Format(CultureInfo.CurrentCulture, "Failed to open file {0}", _fileName), exception);
			}
			XmlNodeList nodeList = sourceFile.GetElementsByTagName("resource");
			_resources = new Resource[nodeList.Count];
			for (int whichResource = 0; whichResource < _resources.Length; ++whichResource)
			{
				try
				{
					_resources[whichResource] = new Resource(nodeList[whichResource]);
				}
				catch (Exception exception)
				{
					throw new Exception(String.Format(CultureInfo.CurrentCulture, "Failed to create resource object number {0}.", whichResource), exception);
				}
			}
		}
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
