namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Parser<TParsable> where TParsable : class, IParsable
    {
        protected IEnumerable<TParsable> Content { get; set; }

        public virtual TParsable Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return this.Content.FirstOrDefault(element => input.Equals(element.Name, StringComparison.OrdinalIgnoreCase));
        }

        public virtual TParsable Find(int? id)
        {
            return id == null ? null : this.Content.FirstOrDefault(x => x.Id == id);
        }
    }
}