using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.ViewModels
{
    public class VehicleViewModel : BaseViewModel
    {
        private readonly SessionService _session;
        public string LicensePlate { get; }
        public string ImageUrl { get; }
        public string Model { get; }
        public string Make { get; }


        public VehicleViewModel(string licensePlate, string imageUrl, string model, string make, SessionService session)
        {
            LicensePlate = licensePlate;
            ImageUrl = imageUrl;
            Model = model;
            Make = make;
            _session = session;
        }
    }
}
