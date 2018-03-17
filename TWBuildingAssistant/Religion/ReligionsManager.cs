using System;
using System.Globalization;
using System.Xml;
namespace Religions
{
	/// <summary>
	/// Obiekt tej klasy zawiera informacje o wszystkich religiach.
	/// </summary>
	public class ReligionsManager
	{
		/// <summary>
		/// Odwołanie do tego jedynego obiektu zarządzającego wszystkimi religiami.
		/// </summary>
		public static ReligionsManager Singleton { get; private set; } = null;
		/// <summary>
		/// Tworzy instancję zbioru religii dostępnej później poprzez właściwość Singleton. Nie wywoływać więcej niż raz.
		/// </summary>
		public static ReligionsManager CreateSingleton()
		{
			if (Singleton == null)
			{
				Singleton = new ReligionsManager();
				return Singleton;
			}
			throw new InvalidOperationException("You cannot create a new singleton when one already exists.");
		}
		//
		//
		//
		private const string _fileName = "twa_religions.xml";
		private readonly ReligionData[] _religions;
		private int _stateReligion;
		/// <summary>
		/// Liczba wszystkich dostępnych religii.
		/// </summary>
		public int ReligionTypesCount
		{
			get { return _religions.Length; }
		}
		private ReligionsManager()
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
			XmlNodeList nodeList = sourceFile.GetElementsByTagName("religion");
			_religions = new ReligionData[nodeList.Count];
			for (int whichReligion = 0; whichReligion < _religions.Length; ++whichReligion)
			{
				try
				{
					_religions[whichReligion] = new ReligionData(nodeList[whichReligion]);
				}
				catch (Exception exception)
				{
					throw new Exception(String.Format(CultureInfo.CurrentCulture, "Failed to create religion object number {0}.", whichReligion), exception);
				}
			}
			PickStateReligion();
		}
		private void PickStateReligion()
		{
			_stateReligion = -1;
			for (int whichReligion = 0; whichReligion < ReligionTypesCount; ++whichReligion)
				Console.WriteLine("{0}. {1}", whichReligion, _religions[whichReligion].ToString());
			while (true)
			{
				Console.Write("Pick your state religion: ");
				try
				{
					_stateReligion = Convert.ToInt32(Console.ReadLine(), CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					_stateReligion = -1;
				}
				if (_stateReligion > -1 && _stateReligion < ReligionTypesCount)
					break;
			}
		}
		/// <summary>
		/// Weryfikuje czy dany tekst jest nazwą któreś religii.
		/// </summary>
		/// <param name="input">Tekst do weryfikacji.</param>
		public ReligionData Parse(string input)
		{
			if (input == null)
				return null;
			if (input == "State")
				return StateReligion;
			foreach (ReligionData religion in _religions)
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
		public int GetIndex(ReligionData religion)
		{
			if (religion == null)
				throw new ArgumentNullException("religion");
			for (int whichReligion = 0; whichReligion < _religions.Length; ++whichReligion)
				if (religion == _religions[whichReligion])
					return whichReligion;
			throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Unknown ReligionData object (name: {0})", religion.Name), "religion");
		}
		/// <summary>
		/// Religia wybrana przez użytkownika jako państwowa.
		/// </summary>
		public ReligionData StateReligion
		{
			get { return _religions[_stateReligion]; }
		}
	}
}