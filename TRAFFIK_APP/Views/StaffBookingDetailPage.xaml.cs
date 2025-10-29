using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.ViewModels;
using Microsoft.Maui.Controls;

namespace TRAFFIK_APP.Views
{
    public partial class StaffBookingDetailPage : ContentPage
    {
        private StaffBookingDetailViewModel? _viewModel;

        public static BookingStageUpdateDto? SelectedBooking { get; set; }

        public StaffBookingDetailPage(StaffBookingDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
            
            // Wire up Shell navigation back button to use ViewModel's command
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                Command = _viewModel.GoBackCommand
            });
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
                            await Shell.Current.GoToAsync("//StaffDashboardPage");
                        }
                    }
                });
            });
            return true; // Prevent default back button behavior
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Load booking data from static property
            if (SelectedBooking != null && _viewModel != null)
            {
                await _viewModel.LoadBookingDetailsAsync(SelectedBooking.BookingId);
            }
        }
    }
}

