using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
    public partial class StaffDashboardPage : ContentPage
    {
        public StaffDashboardPage()
        {
            InitializeComponent();
            BindingContext = new StaffDashboardViewModel();
        }
    }
}