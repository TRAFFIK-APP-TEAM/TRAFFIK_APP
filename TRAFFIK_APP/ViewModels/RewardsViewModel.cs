using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Reward;


namespace TRAFFIK_APP.ViewModels
{
    public class RewardsViewModel : BaseViewModel
    {
        private readonly RewardClient _rewardClient;
        private readonly RewardCatalogClient _catalogClient;
        private readonly SessionService _session;

        public int Points { get; private set; }
        public ObservableCollection<RewardItemDto> AvailableRewards { get; } = new();
        public ObservableCollection<RewardItemDto> LockedRewards { get; } = new();

        public ICommand GoHomeCommand { get; }
        public ICommand GoAppointmentsCommand { get; }
        public ICommand GoRewardsCommand { get; }
        public ICommand GoAccountCommand { get; }

        public RewardsViewModel(SessionService session, RewardClient rewardClient, RewardCatalogClient catalogClient)
        {
            _session = session;
            _rewardClient = rewardClient;
            _catalogClient = catalogClient;

            GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await Shell.Current.GoToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await Shell.Current.GoToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));

            LoadRewards();
        }

        private async void LoadRewards()
        {
            OnPropertyChanged(nameof(Points));

            var catalog = await _catalogClient.GetAllAsync() ?? new List<RewardItemDto>();
            AvailableRewards.Clear();
            LockedRewards.Clear();

            foreach (var item in catalog)
            {
                if (item.Cost <= Points)
                    AvailableRewards.Add(item);
                else
                    LockedRewards.Add(item);
            }
        }
    }
}
