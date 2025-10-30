using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class BookingHistoryPage : ContentPage
{
    private BookingViewModel? ViewModel => BindingContext as BookingViewModel;

    public BookingHistoryPage(BookingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object sender, EventArgs e)
    {
        if (ViewModel?.LoadBookingsCommand?.CanExecute(null) == true)
            ViewModel.LoadBookingsCommand.Execute(null);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Reload bookings when the page appears to show updated statuses
        if (ViewModel != null)
        {
            await ViewModel.LoadBookingsAsync();
        }
    }
}

