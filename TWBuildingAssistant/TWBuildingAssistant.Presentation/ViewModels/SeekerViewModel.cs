namespace TWBuildingAssistant.Presentation.ViewModels;

using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Model;

public class SeekerViewModel 
    : ViewModel
{
    private readonly Province province;

    private readonly IEnumerable<BuildingSlot> slots;

    private bool requireSantitation;

    private int minimalPublicOrder;

    public SeekerViewModel(Province province, IEnumerable<BuildingSlot> slots)
    {
        this.requireSantitation = true;
        this.minimalPublicOrder = 1;
        this.province = province;
        this.slots = slots.ToList();

        this.SeekCommand = new RelayCommand(this.Seek);
        this.PreviousCommand = new RelayCommand(this.Previous);
    }

    public event EventHandler<PreviousTransitionEventArgs> PreviousTransition;

    public bool RequireSantitation
    {
        get => this.requireSantitation;
        set
        {
            if (this.requireSantitation != value)
            {
                this.requireSantitation = value;
                this.OnPropertyChanged(nameof(this.RequireSantitation));
            }
        }
    }

    public int MinimalPublicOrder
    {
        get => this.minimalPublicOrder;
        set
        {
            if (this.minimalPublicOrder != value)
            {
                this.minimalPublicOrder = value;
                this.OnPropertyChanged(nameof(this.MinimalPublicOrder));
            }
        }
    }

    public RelayCommand SeekCommand { get; init; }

    public RelayCommand PreviousCommand { get; init; }

    public void Seek()
    {
        var slots = this.slots.ToList();
        if (this.slots.Any())
        {
            var lastSlot = slots.Last();
            var original = slots.Select(x => x.Building).ToList().AsEnumerable();
            var bestCombination = original.ToList().AsEnumerable();
            var bestWealth = 0d;

            void RecursiveSeek(int slotIndex, IEnumerable<BuildingLevel> combination)
            {
                var slot = slots[slotIndex];
                var options = this.province.Owner.GetBuildingLevelsForSlot(this.province, this.province.Regions.Single(x => x.Slots.Contains(slot)), slot);
                foreach (var option in options)
                {
                    slot.Building = option;
                    var currentCombination = combination.Append(option);
                    if (slot == lastSlot)
                    {
                        var state = this.province.State;
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
                slot.Building = enumerator.Current;
            }
        }

        this.Previous();
    }

    public void Previous()
    {
        this.PreviousTransition?.Invoke(this, new PreviousTransitionEventArgs(this.province));
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

    public class PreviousTransitionEventArgs : EventArgs
    {
        public PreviousTransitionEventArgs(Province province)
        {
            this.Province = province;
        }

        public Province Province { get; }
    }
}