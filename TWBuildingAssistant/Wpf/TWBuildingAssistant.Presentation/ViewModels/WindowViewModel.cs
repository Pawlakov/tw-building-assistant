namespace TWBuildingAssistant.Presentation.ViewModels;

using System;

public class WindowViewModel 
    : ViewModel
{
    public event EventHandler CloseWindow;

    protected void OnCloseWindow()
    {
        this.CloseWindow?.Invoke(this, EventArgs.Empty);
    }
}