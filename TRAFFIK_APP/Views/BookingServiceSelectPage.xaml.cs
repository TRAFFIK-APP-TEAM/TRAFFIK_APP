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
	}
}
