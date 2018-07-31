namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;

    public interface IProvincionalEffect
    {
        int? PublicOrder { get; }

        int? RegularFood { get; }

        int? FertilityDependentFood { get; }

        int Food(int fertility);

        int? ProvincionalSanitation { get; }

        int? ResearchRate { get; }

        int? Growth { get; }

        int? Fertility { get; }

        int? ReligiousOsmosis { get; }

        IEnumerable<IBonus> Bonuses { get; }

        IEnumerable<IInfluence> Influences { get; }

        IProvincionalEffect Aggregate(IProvincionalEffect other);
    }
}