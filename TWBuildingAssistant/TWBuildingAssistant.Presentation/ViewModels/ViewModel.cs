namespace TWBuildingAssistant.Presentation.ViewModels
{
    using ReactiveUI;

    public class ViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModel()
        {
            this.Activator = new ViewModelActivator();
        }

        public ViewModelActivator Activator { get; }
    }
}