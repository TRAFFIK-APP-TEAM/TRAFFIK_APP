using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
	public partial class BookingServiceSelectPage : ContentPage
	{
		public BookingServiceSelectPage(BookingServiceSelectViewModel viewModel)
		{
			InitializeComponent();
			BindingContext = viewModel;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			
			// Reload services when the page appears to ensure vehicle filtering works
			if (BindingContext is BookingServiceSelectViewModel viewModel)
			{
				viewModel.ReloadServicesForVehicle();
			}
		}
	}
}
