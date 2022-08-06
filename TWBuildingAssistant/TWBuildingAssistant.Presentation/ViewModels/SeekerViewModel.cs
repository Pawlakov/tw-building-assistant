namespace TWBuildingAssistant.Presentation.ViewModels;

using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TWBuildingAssistant.Model;
using TWBuildingAssistant.Model.Services;

public class SeekerViewModel
    : ViewModel
{
    private readonly ISeekService seekService;

    private readonly Province province;
    private readonly IEnumerable<BuildingSlot> slots;

    private bool requireSantitation;
    private int minimalPublicOrder;
    private bool locked;

    public SeekerViewModel(ISeekService seekService, Province province, IEnumerable<BuildingSlot> slots)
    {
        this.seekService = seekService;

        this.requireSantitation = true;
        this.minimalPublicOrder = 1;
        this.locked = false;
        this.province = province;
        this.slots = slots.ToList();

        this.SeekCommand = new AsyncRelayCommand(this.Seek, this.SeekEnabled);
        this.PreviousCommand = new RelayCommand(this.Previous, this.PreviousEnabled);
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

    public AsyncRelayCommand SeekCommand { get; init; }

    public RelayCommand PreviousCommand { get; init; }

    private async Task Seek()
    {
        await Task.Run(() =>
        {
            this.locked = true;
            if (this.slots.Any())
            {
                this.seekService.Seek(this.province, this.slots.ToList(), this.MinimalCondition);
            }

            this.locked = false;
        });

        this.Previous();
    }

    private void Previous()
    {
        this.PreviousTransition?.Invoke(this, new PreviousTransitionEventArgs(this.province));
    }

    private bool SeekEnabled()
    {
        return !this.locked;
    }

    private bool PreviousEnabled()
    {
        return !this.locked;
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