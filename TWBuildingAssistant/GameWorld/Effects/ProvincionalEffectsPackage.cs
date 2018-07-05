using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GameWorld.Effects
{
	public class ProvincionalEffectsPackage
	{
		// Instance members:

		private readonly IEnumerable<WealthBonus> _wealthBonuses;

		[JsonConstructor]
		public ProvincionalEffectsPackage(int publicOrder, int food, int provincionalSanitation, int religiousOsmosis, int religiousInfluence, int researchRate, int growth, int fertility, IEnumerable<WealthBonus> wealthBonuses)
		{
			PublicOrder = publicOrder;
			Food = food;
			ProvincionalSanitation = provincionalSanitation;
			ReligiousOsmosis = religiousOsmosis;
			ReligiousInfluence = religiousInfluence;
			ResearchRate = researchRate;
			Growth = growth;
			Fertility = fertility;
			if (wealthBonuses != null)
				_wealthBonuses = wealthBonuses.ToList();
			else
				_wealthBonuses = new List<WealthBonus>();
		}

		public int PublicOrder { get; }

		public int Food { get; }

		public int ProvincionalSanitation { get; }

		public int ReligiousOsmosis { get; }

		public int ReligiousInfluence { get; }

		public int ResearchRate { get; }

		public int Growth { get; }

		public int Fertility { get; }

		public IEnumerable<WealthBonus> WealthBonuses => _wealthBonuses.ToArray();

		public override bool Equals(object obj)
		{
			return obj != null && Equals(obj as ProvincionalEffectsPackage);
		}

		protected bool Equals(ProvincionalEffectsPackage other)
		{
			return other != null && (PublicOrder == other.PublicOrder
			                         && Food == other.Food
			                         && ProvincionalSanitation == other.ProvincionalSanitation
			                         && ReligiousOsmosis == other.ReligiousOsmosis
			                         && ReligiousInfluence == other.ReligiousInfluence
			                         && ResearchRate == other.ResearchRate
			                         && Growth == other.Growth && Fertility == other.Fertility
			                         && WealthBonuses.SequenceEqual(other.WealthBonuses));
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = PublicOrder;
				hashCode = (hashCode * 397) ^ Food;
				hashCode = (hashCode * 397) ^ ProvincionalSanitation;
				hashCode = (hashCode * 397) ^ ReligiousOsmosis;
				hashCode = (hashCode * 397) ^ ReligiousInfluence;
				hashCode = (hashCode * 397) ^ ResearchRate;
				hashCode = (hashCode * 397) ^ Growth;
				hashCode = (hashCode * 397) ^ Fertility;
				hashCode = (hashCode * 397) ^ (_wealthBonuses != null ? _wealthBonuses.GetHashCode() : 0);
				return hashCode;
			}
		}

		// Classifier members:

		public static ProvincionalEffectsPackage Deserialize(string json)
		{
			var settings = new JsonSerializerSettings
			{
			MissingMemberHandling = MissingMemberHandling.Error
			};
			return JsonConvert.DeserializeObject<ProvincionalEffectsPackage>(json, settings);
		}

		public static ProvincionalEffectsPackage operator +(ProvincionalEffectsPackage left, ProvincionalEffectsPackage right)
		{
			return new ProvincionalEffectsPackage(
			left.PublicOrder + right.PublicOrder,
			left.Food + right.Food,
			left.ProvincionalSanitation + right.ProvincionalSanitation,
			left.ReligiousOsmosis + right.ReligiousOsmosis,
			left.ReligiousInfluence + right.ReligiousInfluence,
			left.ResearchRate + right.ResearchRate,
			left.Growth + right.Growth,
			left.Fertility + right.Fertility,
			left.WealthBonuses.Concat(right.WealthBonuses));
		}
	}
}