using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRAFFIK_APP.Services;


namespace TRAFFIK_APP.ViewModels
{
    public class VehicleCardViewModel : BaseViewModel
    {
        private readonly SessionService _session;
        public string LicensePlate { get; }
        public string ImageUrl { get; }
        public VehicleCardViewModel(string licensePlate, string imageUrl, SessionService session)
        {
            LicensePlate = licensePlate;
            ImageUrl = imageUrl;
            _session = session;

        }
    }
}
