using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRAFFIK_APP.Models;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingTrackerViewModel : BaseViewModel
    {
        public ObservableCollection<BookingProgressDto> Booking { get; } = new();

        public bool IsEmpty => Booking.Count == 0;
        public bool IsNotEmpty => Booking.Count > 0;

        public BookingTrackerViewModel()
        {
            Booking.CollectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(IsEmpty));
                OnPropertyChanged(nameof(IsNotEmpty));

            };
        }
    }
}
