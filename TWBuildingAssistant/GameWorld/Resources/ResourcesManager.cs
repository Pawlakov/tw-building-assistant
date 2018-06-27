using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.IO;
namespace GameWorld.Resources
{
	// Klasa do zarządzania wszystkimi zasobami.
	public class ResourcesManager : Map.IResourceParser
	{
		public ResourcesManager()
		{
			if (!Validate(out string message))
				throw new FormatException("Cannot create information on resources: " + message);
			XDocument sourceFile = XDocument.Load(_sourceFile);
			_elements = (from XElement element in sourceFile.Root.Elements() select element).ToDictionary((XElement element) => (string)element.Attribute("n"));
			_resources = new Dictionary<string, Resource>(_elements.Count);
		}
		//
		private const string _sourceFile = "Resources\\twa_resources.xml";
		//
		private readonly Dictionary<string, XElement> _elements;
		private readonly Dictionary<string, Resource> _resources;
		//
		public int ResourceTypesCount
		{
			get { return _elements.Count; }
		}
		public Resource Parse(string input)
		{
			if (input == null)
				return null;
			if (_resources.ContainsKey(input))
				return _resources[input];
			if(_elements.ContainsKey(input))
			{
				_resources.Add(input, new Resource(_elements[input]));
				return _resources[input];
			}
			return null;
		}
		// Sprawdzanie poprawności danych XML.
		public static bool Validate(out string message)
		{
			XDocument document;
			if(!File.Exists(_sourceFile))
			{
				message = "Corresponding file not found.";
				return false;
			}
			document = XDocument.Load(_sourceFile);
			if(document.Root == null || document.Root.Elements().Count() < 1)
			{
				message = "Corresponding file is incomplete.";
				return false;
			}
			foreach (XElement element in document.Root.Elements())
				if(!Resource.ValidateElement(element, out string elementMessage))
				{
					message = "One of XML elements is invalid: " + elementMessage;
					return false;
				}
			message = "Information on resources is valid and complete.";
			return true;
		}
	}
}
