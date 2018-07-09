using System;
using Newtonsoft.Json;

namespace GameWorld.Resources
{
	public class Resource
	{
		// Instance members:

		public string Name { get; }
		public SlotType BuildingType { get; }
		public bool IsMandatory { get; }

		[JsonConstructor]
		public Resource(string name, SlotType? buildingType, bool? isMandatory)
		{
			if (buildingType == null)
				throw new ArgumentNullException(nameof(buildingType));
			if (isMandatory == null)
				throw new ArgumentNullException(nameof(isMandatory));
			if (!ValidateValues(buildingType.Value, isMandatory.Value, out var message))
				throw new FormatException(message);
			Name = name ?? throw new ArgumentNullException(nameof(name));
			BuildingType = buildingType.Value;
			IsMandatory = isMandatory.Value;
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			return obj != null && (ReferenceEquals(this, obj) || Equals(obj as Resource));
		}

		private bool Equals(Resource other)
		{
			return other != null && (string.Equals(Name, other.Name) && BuildingType == other.BuildingType && IsMandatory == other.IsMandatory);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (Name != null ? Name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int) BuildingType;
				hashCode = (hashCode * 397) ^ IsMandatory.GetHashCode();
				return hashCode;
			}
		}

		// Classifier members:

		public static bool ValidateValues(SlotType buildingType, bool isMandatory, out string message)
		{
			if (buildingType == SlotType.General && isMandatory)
			{
				message = "Mandatory replacement of general building.";
				return false;
			}

			message = "Values are valid.";
			return true;
		}
	}
}