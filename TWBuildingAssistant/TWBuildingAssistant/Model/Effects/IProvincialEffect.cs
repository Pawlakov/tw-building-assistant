namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;

    public interface IProvincialEffect
    {
        int PublicOrder { get; }

        int RegularFood { get; }

        int FertilityDependentFood { get; }

        int ProvincialSanitation { get; }

        int ResearchRate { get; }

        int Growth { get; }

        int Fertility { get; }

        int ReligiousOsmosis { get; }

        IEnumerable<IBonus> Bonuses { get; }

        IEnumerable<IInfluence> Influences { get; }

        int Food(int fertility);

        IProvincialEffect Aggregate(IProvincialEffect other);

        bool Validate(out string message);

        IProvincialEffect TakeWorst(IProvincialEffect other);
    }
}