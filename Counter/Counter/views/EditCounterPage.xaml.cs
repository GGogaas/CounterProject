using Counter.models;
using System.Windows.Input;

namespace Counter.views;


public partial class EditCounterPage : ContentPage
{
    public EditCounterPage(Counter.models.EditCounterLogic vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
