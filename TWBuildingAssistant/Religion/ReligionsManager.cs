using System;
using System.Globalization;
using System.Xml.Linq;
using System.Linq;
namespace Religions
{
	/// <summary>
	/// Obiekt tej klasy zawiera informacje o wszystkich religiach.
	/// </summary>
	public class ReligionsManager
	{
		// Singleton:
		//
		/// <summary>
		/// Odwołanie do tego jedynego obiektu zarządzającego wszystkimi religiami.
		/// </summary>
		public static ReligionsManager Singleton
		{
			get
			{
				if (HiddenSingleton == null)
					HiddenSingleton = new ReligionsManager();
				return HiddenSingleton;
			}
		}
		private static ReligionsManager HiddenSingleton { get; set; } = null;
		private ReligionsManager()
		{
			XDocument sourceFile = XDocument.Load(_sourceFilename);
			try
			{
				_religions = (from XElement element in sourceFile.Root.Elements() select new Religion(element)).ToArray();
			}
			catch (Exception exception)
			{
				throw new Exception("Failed to create religion objects.", exception);
			}
			StateReligionRelay = new StateReligion();
		}
		//
		// Interfejs publiczny:
		//
		public event StateReligionChangingHandler StateReligionChanging;
		public event StateReligionChangedHandler StateReligionChanged;
		/// <summary>
		/// Liczba wszystkich dostępnych religii.
		/// </summary>
		public int ReligionTypesCount
		{
			get { return _religions.Length; }
		}
		/// <summary>
		/// Weryfikuje czy dany tekst jest nazwą któreś religii.
		/// </summary>
		/// <param name="input">Tekst do weryfikacji.</param>
		public IReligion Parse(string input)
		{
			if (input == null)
				return null;
			if (input == "State")
				return StateReligionRelay;
			foreach (Religion religion in _religions)
			{
				if (religion.Name == input)
					return religion;
			}
			return null;
		}
		/// <summary>
		/// Operator konwersji do liczby całkowitej.
		/// </summary>
		/// <param name="religion">Konwertowana wartość.</param>
		public int GetIndex(IReligion religion)
		{
			if (religion == null)
				throw new ArgumentNullException("religion");
			for (int whichReligion = 0; whichReligion < ReligionTypesCount; ++whichReligion)
				if (religion == _religions[whichReligion])
					return whichReligion;
			if (religion == StateReligionRelay)
				return GetIndex(StateReligion);
			throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Unknown ReligionData object (name: {0})", religion.Name), "religion");
		}
		/// <summary>
		/// Religia wybrana przez użytkownika jako państwowa.
		/// </summary>
		public Religion StateReligion
		{
			get { return _religions[StateReligionIndex]; }
		}
		public void ChangeStateReligion()
		{
			OnStateReligionChanging();
			Console.WriteLine("Pick your state religion:");
			for (int whichReligion = 0; whichReligion < ReligionTypesCount; ++whichReligion)
				Console.WriteLine("{0}. {1}", whichReligion, _religions[whichReligion].ToString());
			do
			{
				Console.Write("From 0 to {0}: ", ReligionTypesCount - 1);
				try
				{
					StateReligionIndex = Convert.ToInt32(Console.ReadLine(), CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					StateReligionIndex = -1;
				}
			} while (StateReligionIndex < 0 || StateReligionIndex > (ReligionTypesCount - 1));
			OnStateReligionChanged();
		}
		//
		// Stałe:
		//
		private const string _sourceFilename = "twa_religions.xml";
		//
		// Stan wewnętrzny:
		//
		private readonly Religion[] _religions;
		private StateReligion StateReligionRelay { get; }
		private int StateReligionIndex { get; set; } = -1;
		//
		// Pomocnicze:
		//
		private void OnStateReligionChanging()
		{
			StateReligionChanging?.Invoke(this, EventArgs.Empty);
		}
		private void OnStateReligionChanged()
		{
			StateReligionChanged?.Invoke(this, EventArgs.Empty);
		}
	}
	public delegate void StateReligionChangingHandler(ReligionsManager sender, EventArgs e);
	public delegate void StateReligionChangedHandler(ReligionsManager sender, EventArgs e);
}