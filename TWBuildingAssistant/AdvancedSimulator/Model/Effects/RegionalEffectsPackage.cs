namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    public class RegionalEffectsPackage : ProvincionalEffectsPackage
    {
        public RegionalEffectsPackage(int publicOrder, int food, int regionalSanitation, int provincionalSanitation, int religiousOsmosis, int religiousInfluence, int researchRate, int growth, int fertility, IEnumerable<WealthBonus> wealthBonuses)
        : base(publicOrder, food, provincionalSanitation, religiousOsmosis, religiousInfluence, researchRate, growth, fertility, wealthBonuses)
        {
            RegionalSanitation = regionalSanitation;
        }

        public int RegionalSanitation { get; }

        public override bool Equals(object obj)
        {
            return obj != null && Equals(obj as RegionalEffectsPackage);
        }

        private bool Equals(RegionalEffectsPackage other)
        {
            return other != null && RegionalSanitation == other.RegionalSanitation && base.Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ RegionalSanitation;
            }
        }

        public new static RegionalEffectsPackage Deserialize(string json)
        {
            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error
            };
            return JsonConvert.DeserializeObject<RegionalEffectsPackage>(json, settings);
        }

        public static RegionalEffectsPackage operator +(RegionalEffectsPackage left, RegionalEffectsPackage right)
        {
            return new RegionalEffectsPackage(
            left.PublicOrder + right.PublicOrder,
            left.Food + right.Food,
            left.RegionalSanitation + right.RegionalSanitation,
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