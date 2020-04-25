namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using TWBuildingAssistant.Data.Sqlite;

    public class World
    {
        public World()
        {
            using (var context = new DatabaseContext())
            {
                var resources = new List<KeyValuePair<int, Resource>>();
                foreach (var resourceEntity in context.Resources.ToList())
                {
                    resources.Add(new KeyValuePair<int, Resource>(resourceEntity.Id, new Resource(resourceEntity.Name)));
                }

                var religions = new List<KeyValuePair<int, Religion>>();
                foreach (var religionEntity in context.Religions.ToList())
                {
                    Effect effect = default;
                    if (religionEntity.EffectId.HasValue)
                    {
                        var effectEntity = context.Effects.Find(religionEntity.EffectId.Value);
                        var bonusEntities = context.Bonuses.Where(x => x.EffectId == effectEntity.Id).ToList();
                        var influenceEntities = context.Influences.Where(x => x.EffectId == effectEntity.Id).ToList();
                        if (influenceEntities.Any(x => x.ReligionId.HasValue))
                        {
                            throw new DomainRuleViolationException("Influence of a religion with a set religion id.");
                        }

                        var influence = influenceEntities.Select(x => new Influence(null, x.Value)).Aggregate(default(Influence), (x, y) => x + y);
                        var bonus = bonusEntities.Select(x => new Income(x.Value, x.Category, x.Type)).Aggregate(default(Income), (x, y) => x + y);
                        effect = new Effect(effectEntity.PublicOrder, effectEntity.RegularFood, effectEntity.FertilityDependentFood, effectEntity.ProvincialSanitation, effectEntity.ResearchRate, effectEntity.Growth, effectEntity.Fertility, effectEntity.ReligiousOsmosis, 0, bonus, influence);
                    }

                    religions.Add(new KeyValuePair<int, Religion>(religionEntity.Id, new Religion(religionEntity.Name, effect)));
                }

                var provinces = new List<KeyValuePair<int, Province>>();
                foreach (var provinceEntity in context.Provinces.ToList())
                {
                    Effect effect = default;
                    if (provinceEntity.EffectId.HasValue)
                    {
                        var effectEntity = context.Effects.Find(provinceEntity.EffectId);
                        var bonusEntities = context.Bonuses.Where(x => x.EffectId == effectEntity.Id).ToList();
                        var influenceEntities = context.Influences.Where(x => x.EffectId == effectEntity.Id).ToList();

                        var influence = influenceEntities.Select(x => new Influence(x.ReligionId.HasValue ? religions.Single(y => y.Key == x.ReligionId).Value : null, x.Value)).Aggregate(default(Influence), (x, y) => x + y);
                        var bonus = bonusEntities.Select(x => new Income(x.Value, x.Category, x.Type)).Aggregate(default(Income), (x, y) => x + y);
                        effect = new Effect(effectEntity.PublicOrder, effectEntity.RegularFood, effectEntity.FertilityDependentFood, effectEntity.ProvincialSanitation, effectEntity.ResearchRate, effectEntity.Growth, effectEntity.Fertility, effectEntity.ReligiousOsmosis, 0, bonus, influence);
                    }

                    var regions = new List<Region>();
                    foreach (var regionEntity in context.Regions.Where(x => x.ProvinceId == provinceEntity.Id).ToList())
                    {
                        regions.Add(new Region(regionEntity.Name, regionEntity.RegionType, regionEntity.IsCoastal, regionEntity.ResourceId.HasValue ? resources.Single(x => x.Key == regionEntity.ResourceId).Value : null, regionEntity.SlotsCountOffset == -1));
                    }

                    provinces.Add(new KeyValuePair<int, Province>(provinceEntity.Id, new Province(provinceEntity.Name, regions, effect)));
                }

                var factions = new List<KeyValuePair<int, Faction>>();
                foreach (var factionEntity in context.Factions.ToList())
                {
                    factions.Add(new KeyValuePair<int, Faction>(factionEntity.Id, new Faction(factionEntity.Name)));
                }

                this.Resources = resources.Select(x => x.Value);
                this.Religions = religions.Select(x => x.Value);
                this.Provinces = provinces.Select(x => x.Value);
                this.Factions = factions.Select(x => x.Value);
            }
        }

        public IEnumerable<Resource> Resources { get; }

        public IEnumerable<Religion> Religions { get; }

        public IEnumerable<Province> Provinces { get; }

        public IEnumerable<Faction> Factions { get; }
    }
}