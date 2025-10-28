namespace Counter.views;

public partial class AddCounterPage : ContentPage
{
	public AddCounterPage()
	{
		InitializeComponent();
	}

    private void btnGoBack_Clicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("..");
    }
}