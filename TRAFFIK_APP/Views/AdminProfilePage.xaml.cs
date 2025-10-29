using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.Views
{
    public partial class AdminProfilePage : ContentPage
    {
        public AdminProfilePage(AdminProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}

