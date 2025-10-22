using System;
using System.Windows.Input;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingDateTimeSelectViewModel : BaseViewModel
    {
        private DateTime _selectedDate = DateTime.Today;
        private TimeSpan _selectedTime = DateTime.Now.TimeOfDay;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public TimeSpan SelectedTime
        {
            get => _selectedTime;
            set => SetProperty(ref _selectedTime, value);
        }

        public DateTime Today => DateTime.Today;

        public ICommand ContinueCommand { get; }

        public BookingDateTimeSelectViewModel()
        {
            ContinueCommand = new Command(async () => await OnContinue());
        }

        private async Task OnContinue()
        {
            // Combine date + time for confirmation
            var selectedDateTime = SelectedDate.Date + SelectedTime;
            await Shell.Current.GoToAsync("//BookingConfirmationPage",
                new Dictionary<string, object>
                {
                    { "SelectedDateTime", selectedDateTime }
                });
        }
    }
}
