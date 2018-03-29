using System;
using System.Xml.Linq;
using System.Globalization;
namespace Map
{
	/// <summary>
	/// Zbiór tradycji religijnych prowincji.
	/// </summary>
	public class ProvinceTraditions
	{
		// Stan wewnętrzny:
		//
		private readonly int[] _traditions;
		//
		// Interfejs zewnętrzny:
		//
		/// <summary>
		/// Tworzy zbiór tradycji religijnych dla prowincji.
		/// </summary>
		/// <param name="element">Element XML zawierający niezbędne informacje.</param>
		internal ProvinceTraditions(XElement element)
		{
			_traditions = new int[Religions.ReligionsManager.Singleton.ReligionTypesCount];
			foreach (XElement traditionElement in element.Elements())
			{
				string innerText = (string)traditionElement.Attribute("r");
				Religions.IReligion religion = Religions.ReligionsManager.Singleton.Parse(innerText);
				if (religion == null)
					throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Value {0} was not recognized as any religion.", innerText));
				_traditions[Religions.ReligionsManager.Singleton.GetIndex(religion)] += (int)traditionElement;
			}
			CurrentStateReligion = Religions.ReligionsManager.Singleton.StateReligion;
			Religions.ReligionsManager.Singleton.StateReligionChanged += (Religions.ReligionsManager sender, EventArgs e) => { CurrentStateReligion = sender.StateReligion; };
		}
		/// <summary>
		/// Zwraca wpływ danej religii w tej prowincji.
		/// </summary>
		/// <param name="religion">Dana religia.</param>
		/// <returns>Wpływ dla podanej religii.</returns>
		public int StateReligionTradition
		{
			get
			{
				if (CurrentStateReligion == null)
					throw new InvalidOperationException("State religion is unknown.");
				return _traditions[Religions.ReligionsManager.Singleton.GetIndex(CurrentStateReligion)];
			}
		}
		/// <summary>
		/// Zwraca wpływ wszystkich religii poza daną w tej prowincji.
		/// </summary>
		/// <param name="religion">Dana religia.</param>
		/// <returns>Wpływ dla wszystkich religii poza podaną.</returns>
		public int OtherReligionsTradition
		{
			get
			{
				if (CurrentStateReligion == null)
					throw new InvalidOperationException("State religion is unknown.");
				int result = 0;
				for (int whichReligion = 0; whichReligion < Religions.ReligionsManager.Singleton.ReligionTypesCount; ++whichReligion)
					if (whichReligion != Religions.ReligionsManager.Singleton.GetIndex(CurrentStateReligion))
						result += _traditions[whichReligion];
				return result;
			}
		}
		//
		// Stan wewnętrzny:
		//
		private Religions.Religion CurrentStateReligion { get; set; }
	}
}