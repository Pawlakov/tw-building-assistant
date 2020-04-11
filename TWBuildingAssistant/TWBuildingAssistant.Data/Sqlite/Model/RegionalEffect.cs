namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class RegionalEffect : ProvincialEffect, IRegionalEffect
    {
        public RegionalEffect()
            : base()
        {
        }

        public RegionalEffect(IRegionalEffect source)
            : base(source)
        {
            this.RegionalSanitation = source.RegionalSanitation;
        }

        public int RegionalSanitation { get; set; }
    }
}