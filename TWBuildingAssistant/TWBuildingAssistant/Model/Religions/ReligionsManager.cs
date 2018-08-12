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
            this.religions = source.GetReligions();
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

        public IEnumerable<IReligion> Religions => this.religions.ToArray();

        public IEnumerable<KeyValuePair<int, string>> AllReligionsNames
        {
            get
            {
                var result = new List<KeyValuePair<int, string>>(this.religions.Count());
                var whichReligion = 0;
                foreach (var religion in this.religions)
                {
                    result.Add(new KeyValuePair<int, string>(whichReligion, religion.Name));
                    ++whichReligion;
                }

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
            if (whichReligion < 0 || whichReligion > (this.religions.Count() - 1))
            {
                throw new ArgumentOutOfRangeException(
                nameof(whichReligion),
                whichReligion,
                "The index of new state religion is out of range.");
            }

            this.StateReligion = this.religions.ToArray()[whichReligion];
            this.OnStateReligionChanged(new StateReligionChangedArgs(this, this.StateReligion));
        }

        private void OnStateReligionChanged(StateReligionChangedArgs e)
        {
            this.StateReligionChanged?.Invoke(this, e);
        }
    }

    public partial class ReligionsManager : IReligionParser
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
    }
}
