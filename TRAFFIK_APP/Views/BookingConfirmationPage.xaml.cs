using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class BookingConfirmationPage : ContentPage
{
	public BookingConfirmationPage(BookingConfirmationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
