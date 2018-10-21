namespace TWBuildingAssistant.Model.Religions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Unity;

    public partial class ReligionsManager : Parser<IReligion>
    {
        public ReligionsManager(IUnityContainer resolver)
        {
            this.Content = resolver.Resolve<ISource>().Religions.ToArray();
            foreach (var religion in this.Content)
            {
                religion.StateReligionTracker = this;
                foreach (var influence in religion.Effect.Influences)
                {
                    influence.SetReligionParser(this);
                }
            }

            var message = string.Empty;
            if (this.Content.Any(religion => !religion.Validate(out message)))
            {
                throw new ReligionsException($"One of religions is not valid ({message}).");
            }
        }

        public IEnumerable<KeyValuePair<int, string>> AllReligionsNames
        {
            get
            {
                var result = this.Content.Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
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
            var newStateReligion = this.Content.FirstOrDefault(x => x.Id == whichReligion);
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
}
