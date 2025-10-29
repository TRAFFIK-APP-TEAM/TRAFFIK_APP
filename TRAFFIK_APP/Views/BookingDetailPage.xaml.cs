using TRAFFIK_APP.ViewModels;
using Microsoft.Maui.Controls;

namespace TRAFFIK_APP.Views
{
    public partial class BookingDetailPage : ContentPage
    {
        public static TRAFFIK_APP.Models.Dtos.Booking.BookingDto? SelectedBooking { get; set; }
        public static string? SourcePage { get; set; } // Track where we came from
        private BookingDetailViewModel? _viewModel;

        public BookingDetailPage()
        {
            InitializeComponent();
            
            if (SelectedBooking != null)
            {
                _viewModel = new BookingDetailViewModel(SelectedBooking);
                BindingContext = _viewModel;
                
                // Wire up Shell navigation back button to use ViewModel's command
                Shell.SetBackButtonBehavior(this, new BackButtonBehavior
                {
                    Command = _viewModel.GoBackCommand
                });
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Reload stages when the page appears to show updated progress
            if (_viewModel != null)
            {
                await _viewModel.LoadStagesAsync();
            }
            
            // Update back command based on source
            if (_viewModel != null && !string.IsNullOrEmpty(SourcePage))
            {
                if (SourcePage == "AdminManageBookingsPage")
                {
                    // Navigate back to AdminManageBookingsPage using unambiguous route
                    _viewModel.SetBackNavigation(async () => await Shell.Current.GoToAsync("admin_manage_bookings"));
                }
                // Else use default PopAsync behavior
                
                // Update Shell back button behavior after setting back navigation
                Shell.SetBackButtonBehavior(this, new BackButtonBehavior
                {
                    Command = _viewModel.GoBackCommand
                });
            }
        }

        protected override bool OnBackButtonPressed()
        {
            // Handle hardware back button - navigate back in the stack
            _ = Task.Run(async () =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        // Try Navigation stack first (if we were pushed)
                        var currentPage = Shell.Current?.CurrentPage;
                        if (currentPage?.Navigation?.NavigationStack?.Count > 1)
                        {
                            await currentPage.Navigation.PopAsync();
                            return;
                        }
                        
                        // Fallback to Shell navigation
                        if (Shell.Current.Navigation.NavigationStack.Count > 1)
                        {
                            await Shell.Current.Navigation.PopAsync();
                        }
                        else
                        {
                            // Use relative navigation
                            await Shell.Current.GoToAsync("..");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[BackButton] Error: {ex.Message}");
                        // Fallback to relative navigation
                        try
                        {
                            await Shell.Current.GoToAsync("..");
                        }
                        catch
                        {
                            // Navigate to dashboard as last resort
                            await Shell.Current.GoToAsync("//DashboardPage");
                        }
                    }
                });
            });
            return true; // Prevent default back button behavior
        }
    }
}
