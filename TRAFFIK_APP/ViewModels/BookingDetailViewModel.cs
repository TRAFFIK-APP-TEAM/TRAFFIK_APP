using System.Collections.ObjectModel;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Services.ApiClients;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingDetailViewModel : BaseViewModel
    {
        private readonly BookingStagesClient _bookingStagesClient;
        
        public BookingDto Booking { get; private set; }
        public ObservableCollection<BookingStageUpdateDto> Stages { get; } = new();
        
        public string CurrentStage { get; private set; } = "Pending";
        public double Progress => CalculateProgress();

        // Stage properties for tracking
        public string Stage2Color => GetStageColor(2);
        public string Stage3Color => GetStageColor(3);
        public string Stage4Color => GetStageColor(4);
        public string Stage5Color => GetStageColor(5);
        
        public string Stage2Text => "Service Started";
        public string Stage3Text => "Wash Completed";
        public string Stage4Text => "Dry Completed";
        public string Stage5Text => "Service Completed";

        public ICommand GoBackCommand { get; }

        private readonly List<string> StageSequence = new()
        {
            "Pending",
            "Service Started",
            "Wash Completed",
            "Dry Completed",
            "Service Completed"
        };

        public BookingDetailViewModel(BookingDto booking)
        {
            Booking = booking;
            
            // Try to get BookingStagesClient from dependency injection
            _bookingStagesClient = Helpers.ServiceHelper.GetService<BookingStagesClient>();
            
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            
            // Load stages when view model is created
            _ = LoadStagesAsync();
        }

        private async Task LoadStagesAsync()
        {
            if (_bookingStagesClient == null) return;
            
            try
            {
                var stages = await _bookingStagesClient.GetByBookingAsync(Booking.Id);
                if (stages != null)
                {
                    Stages.Clear();
                    foreach (var stage in stages)
                    {
                        Stages.Add(stage);
                    }
                    
                    // Update current stage
                    if (Stages.Count > 0)
                    {
                    var latestStage = Stages.OrderByDescending(s => s.UpdatedAt).FirstOrDefault();
                    if (latestStage != null)
                    {
                        CurrentStage = latestStage.SelectedStage ?? latestStage.CurrentStage ?? "Pending";
                    }
                    }
                    
                    OnPropertyChanged(nameof(CurrentStage));
                    OnPropertyChanged(nameof(Progress));
                    OnPropertyChanged(nameof(Stage2Color));
                    OnPropertyChanged(nameof(Stage3Color));
                    OnPropertyChanged(nameof(Stage4Color));
                    OnPropertyChanged(nameof(Stage5Color));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BookingDetailViewModel] Error loading stages: {ex.Message}");
            }
        }

        private double CalculateProgress()
        {
            var index = StageSequence.IndexOf(CurrentStage);
            if (index < 0) return 0;
            return (index + 1) / (double)StageSequence.Count;
        }

        private string GetStageColor(int stageIndex)
        {
            var currentIndex = StageSequence.IndexOf(CurrentStage);
            if (stageIndex <= currentIndex)
                return "#007AFF"; // Blue for completed stages
            else
                return "#3E3E3E"; // Gray for pending stages
        }
    }

}
