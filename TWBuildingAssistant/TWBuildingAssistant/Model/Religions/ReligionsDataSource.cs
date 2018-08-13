namespace TWBuildingAssistant.Model.Religions
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Provides acces to all <see cref="IReligion"/> objects found in the datbase.
    /// </summary>
    public class ReligionsDataSource : IReligionsSource
    {
        /// <summary>
        /// The database context containing information on religions.
        /// </summary>
        private readonly Data.DataModel context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReligionsDataSource"/> class.
        /// </summary>
        /// <param name="context">
        /// The database context containing information on religions.
        /// </param>
        public ReligionsDataSource(Data.DataModel context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets all religions from the database.
        /// </summary>
        /// <returns>
        /// All <see cref="IReligion"/> objects from the database.
        /// </returns>
        public IEnumerable<IReligion> GetReligions()
        {
            IEnumerable<IReligion> religions = (from Religion item in this.context.Religions select item).ToArray();
            var effects = (from ReligionEffect item in this.context.ReligionEffects select item).ToArray();
            var bonuses = (from ReligionBonus item in this.context.ReligionBonuses select item).ToArray();
            var influences = (from ReligionInfluence item in this.context.ReligionInfluences select item).ToArray();
            return religions;
        }
    }
}