namespace TWBuildingAssistant.Model.Religions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ReligionsDataSource : IReligionsSource
    {
        private readonly Data.DataModel context;

        public ReligionsDataSource(Data.DataModel context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<IReligion> GetReligions()
        {
            return (from Religion item in this.context.Religions select item).ToArray();
        }
    }
}