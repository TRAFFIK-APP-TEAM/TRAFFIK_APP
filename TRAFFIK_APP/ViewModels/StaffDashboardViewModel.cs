using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TRAFFIK_APP.Models;


namespace TRAFFIK_APP.ViewModels
{
    public class StaffDashboardViewModel : BaseViewModel
    {
        public ObservableCollection<BookingStageUpdateDto> ActiveBookings { get; set; } = new();

        public ICommand UpdateStageCommand { get; }

        public StaffDashboardViewModel()
        {
            UpdateStageCommand = new Command<BookingStageUpdateDto>((booking) =>
            {
                //API TO BE PLUGGED IN HERE 
            });
        }
    }

   
}
