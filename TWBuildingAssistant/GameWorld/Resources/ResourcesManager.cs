using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameWorld.Resources
{
	public class ResourcesManager : Map.IResourceParser
	{
		//Instance members:

		private const string SourceFile = "Resources\\twa_resources.json";
		private readonly IEnumerable<Resource> _resources;

		public ResourcesManager()
		{
			var settings = new JsonSerializerSettings {MissingMemberHandling = MissingMemberHandling.Error};
			var json = File.ReadAllText(SourceFile);
			_resources = JsonConvert.DeserializeObject<Resource[]>(json, settings);
			Count = _resources.Count();
		}

		public int Count { get; }

		public Resource Parse(string input)
		{
			if (input == null)
				return null;
			foreach (var element in _resources)
				if (element.Name.Equals(input))
					return element;
			return null;
		}
	}
}