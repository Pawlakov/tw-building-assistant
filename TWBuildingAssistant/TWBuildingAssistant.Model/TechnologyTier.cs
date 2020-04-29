namespace TWBuildingAssistant.Model
{
    public class TechnologyTier
    {
        public TechnologyTier(Effect universalEffect, Effect antilegacyEffect = default)
        {
            this.UniversalEffect = universalEffect;
            this.AntilegacyEffect = antilegacyEffect;
        }

        public Effect UniversalEffect { get; }

        public Effect AntilegacyEffect { get; }
    }
}