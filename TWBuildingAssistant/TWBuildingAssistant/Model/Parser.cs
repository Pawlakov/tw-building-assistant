namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Parser<TParseable> where TParseable : class, IParsable
    {
        protected IEnumerable<TParseable> Content { get; set; }

        public virtual TParseable Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return this.Content.FirstOrDefault(element => input.Equals(element.Name, StringComparison.OrdinalIgnoreCase));
        }

        public virtual TParseable Find(int? id)
        {
            return id == null ? null : this.Content.FirstOrDefault(x => x.Id == id);
        }
    }
}