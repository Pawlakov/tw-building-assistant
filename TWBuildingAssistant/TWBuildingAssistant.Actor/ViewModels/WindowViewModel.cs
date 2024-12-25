namespace TWBuildingAssistant.Actor.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WindowViewModel
    : ViewModel
{
    public event EventHandler CloseWindow;

    protected void OnCloseWindow()
    {
        this.CloseWindow?.Invoke(this, EventArgs.Empty);
    }
}