namespace TWBuildingAssistant.Data.Model
{
    public interface IProvincialEffect
    {
        int Id { get; }

        int PublicOrder { get; }

        int RegularFood { get; }

        int FertilityDependentFood { get; }

        int ProvincialSanitation { get; }

        int ResearchRate { get; }

        int Growth { get; }

        int Fertility { get; }

        int ReligiousOsmosis { get; }
    }
}