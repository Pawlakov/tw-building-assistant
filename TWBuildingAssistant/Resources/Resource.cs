using System.Xml.Linq;
namespace Resources
{
	/// <summary>
	/// Obiekt tej klasy zawiera informacje na temat jednego z zasobów dostępnych w grze.
	/// </summary>
	public class Resource
	{
		// Interfejs publiczny / Stan wewnętrzny:
		//
		/// <summary>
		/// Nazwa zasobu.
		/// </summary>
		public string Name { get; }
		//
		// Interfejs wewnętrzny:
		//
		internal Resource(XElement element)
		{
			Name = (string)element.Attribute("n");
		}
	}
}
