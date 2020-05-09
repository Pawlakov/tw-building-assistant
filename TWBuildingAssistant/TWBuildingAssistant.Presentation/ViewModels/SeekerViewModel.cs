namespace TWBuildingAssistant.Presentation.ViewModels
{
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Linq;
    using TWBuildingAssistant.Model;
    using TWBuildingAssistant.Presentation.Views;

    public class SeekerViewModel : ViewModel
    {
        private bool requireSantitation;

        private int minimalPublicOrder;

        private ProvinceViewModel province;

        public SeekerViewModel(ProvinceViewModel province)
        {
            this.requireSantitation = true;
            this.minimalPublicOrder = 1;
            this.province = province;
        }

        public bool RequireSantitation
        {
            get => this.requireSantitation;
            set
            {
                this.RaiseAndSetIfChanged(ref this.requireSantitation, value);
            }
        }

        public int MinimalPublicOrder
        {
            get => this.minimalPublicOrder;
            set
            {
                this.RaiseAndSetIfChanged(ref this.minimalPublicOrder, value);
            }
        }

        public void Seek()
        {
            var slots = this.province.Regions.SelectMany(x => x.Slots.Where(y => y.Seek)).ToList();
            if (slots.Any())
            {
                var lastSlot = slots.Last();
                var original = slots.Select(x => x.SelectedBuilding).ToList().AsEnumerable();
                var bestCombination = original.ToList().AsEnumerable();
                var bestWealth = 0d;

                // Weź może enumeruj po tych slotach co?
                void RecursiveSeek(int slotIndex, IEnumerable<BuildingLevel> combination)
                {
                    var slot = slots[slotIndex];
                    var options = slot.Buildings.ToList();
                    foreach (var option in options)
                    {
                        slot.SelectedBuilding = option;
                        var currentCombination = combination.Append(option);
                        if (slot == lastSlot)
                        {
                            var state = this.province.CurrentState;
                            if (this.MinimalCondition(state) && state.Wealth > bestWealth)
                            {
                                bestWealth = state.Wealth;
                                bestCombination = currentCombination;
                            }
                        }
                        else
                        {
                            RecursiveSeek(slotIndex + 1, currentCombination);
                        }
                    }
                }

                RecursiveSeek(0, new List<BuildingLevel>());
                var enumerator = bestCombination.GetEnumerator();
                foreach (var slot in slots)
                {
                    enumerator.MoveNext();
                    slot.SelectedBuilding = enumerator.Current;
                }
            }
        }

        private bool MinimalCondition(ProvinceState state)
        {
            if (state.Food < 0)
            {
                return false;
            }

            if (this.requireSantitation && state.Sanitation.Any(x => x < 0))
            {
                return false;
            }

            if (state.PublicOrder < this.minimalPublicOrder)
            {
                return false;
            }

            return true;
        }
    }
}