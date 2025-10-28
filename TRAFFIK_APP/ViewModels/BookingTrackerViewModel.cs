using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRAFFIK_APP.Models.Dtos.Booking;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingTrackerViewModel : BaseViewModel
    {
        public ObservableCollection<BookingProgressDto> Bookings { get; } = new();

        public bool IsEmpty => Bookings.Count == 0;
        public bool IsNotEmpty => Bookings.Count > 0;

        public BookingTrackerViewModel()
        {
            Bookings.CollectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(IsEmpty));
                OnPropertyChanged(nameof(IsNotEmpty));

            };
        }
    }
}
