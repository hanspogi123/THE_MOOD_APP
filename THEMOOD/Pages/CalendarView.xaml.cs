using THEMOOD.ViewModels;

namespace THEMOOD.Pages;

public partial class CalendarView : ContentPage
{
	public CalendarView()
	{
		InitializeComponent();

        // Connect the NavBar control with the singleton ViewModel
		NavBar.SetViewModel(THEMOOD.ViewModels.NavBarViewModel.Instance);
        Shell.SetNavBarIsVisible(this, false);
    }

	private void MoodFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(BindingContext is CalendarView_VM viewModel)
        {
            viewModel.FilterMoodsCommand.Execute(null);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if(BindingContext is CalendarView_VM viewModel)
        {
            viewModel.FilterMoodsCommand.Execute(null);
        }
    }
}