using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class StaffViewCodesPage : ContentPage
{
    public StaffViewCodesPage(StaffViewCodesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        
        // Wire up the Shell navigation bar back button
        Shell.SetBackButtonBehavior(this, new Microsoft.Maui.Controls.BackButtonBehavior
        {
            Command = viewModel.GoBackCommand
        });
    }

    protected override bool OnBackButtonPressed()
    {
        // Navigate back when hardware/gesture back button is pressed
        Shell.Current.GoToAsync("..").Wait();
        return true; // Prevent default back button behavior
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Reload codes when page appears
        if (BindingContext is StaffViewCodesViewModel viewModel)
        {
            await viewModel.LoadActiveCodesAsync();
        }
    }
}

