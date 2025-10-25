using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRAFFIK_APP.Models.Dtos.Car
{
    public class CarModelCreateDto
    {
        public int UserId { get; set; }
        public string VehicleType { get; set; } = null!;
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string PlateNumber { get; set; } = null!;
        public int Year { get; set; }
    }
}
