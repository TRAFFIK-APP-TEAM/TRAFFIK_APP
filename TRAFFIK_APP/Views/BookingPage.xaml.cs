using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class BookingPage : ContentPage
{
    private BookingViewModel ViewModel => BindingContext as BookingViewModel;

    public BookingPage(BookingViewModel viewModel)
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
}
