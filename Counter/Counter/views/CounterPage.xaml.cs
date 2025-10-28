using Counter.models;


namespace Counter.views;

public partial class CountersPage : ContentPage
{
    readonly Counter.models.CountersLogic _vm;

    public CountersPage(Counter.models.CountersLogic vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.RefreshAsync();
    }
}