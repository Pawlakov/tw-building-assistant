namespace TWBuildingAssistant.Model.Religions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ReligionsManager
    {
        private readonly IEnumerable<IReligion> religions;

        public ReligionsManager(IReligionsSource source)
        {
            this.religions = source.Religions.ToArray();
            foreach (var religion in this.religions)
            {
                foreach (var influence in religion.Effect.Influences)
                {
                    influence.ReligionParser = this;
                }
            }

            var message = string.Empty;
            if (this.religions.Any(resource => !resource.Validate(out message)))
            {
                throw new ReligionsException($"One of religions is not valid ({message}).");
            }

            foreach (var religion in this.religions)
            {
                religion.StateReligionTracker = this;
            }
        }

        public IEnumerable<KeyValuePair<int, string>> AllReligionsNames
        {
            get
            {
                var result = this.religions.Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
                return result;
            }
        }
    }

    public partial class ReligionsManager : IStateReligionTracker
    {
        public event EventHandler<StateReligionChangedArgs> StateReligionChanged;

        public IReligion StateReligion { get; private set; }

        public void ChangeStateReligion(int whichReligion)
        {
            var newStateReligion = this.religions.FirstOrDefault(x => x.Id == whichReligion);
            this.StateReligion = newStateReligion ?? throw new ArgumentOutOfRangeException(
                                     nameof(whichReligion),
                                     whichReligion,
                                     $"There is no religions with id={whichReligion}.");
            this.OnStateReligionChanged(new StateReligionChangedArgs(this, this.StateReligion));
        }

        private void OnStateReligionChanged(StateReligionChangedArgs e)
        {
            this.StateReligionChanged?.Invoke(this, e);
        }
    }

    public partial class ReligionsManager : IParser<IReligion>
    {
        public IReligion Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.Equals("State", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var result = this.religions.FirstOrDefault(element => input.Equals(element.Name, StringComparison.OrdinalIgnoreCase));
            if (result == null)
            {
                throw new ReligionsException("No matching religion found.");
            }

            return result;
        }

        public IReligion Find(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var result = this.religions.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                throw new ReligionsException("No matching religion found.");
            }

            return result;
        }
    }
}
