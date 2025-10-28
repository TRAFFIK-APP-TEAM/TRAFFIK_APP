using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Reward;
using TRAFFIK_APP.Models.Entities.Reward;


namespace TRAFFIK_APP.ViewModels
{
    public class RewardsViewModel : BaseViewModel
    {
        private readonly RewardClient _rewardClient;
        private readonly RewardCatalogClient _catalogClient;
        private readonly SessionService _session;

        private int _points;
        public int Points 
        { 
            get => _points; 
            private set 
            { 
                _points = value;
                OnPropertyChanged();
            } 
        }

        public ObservableCollection<RewardItemDto> AvailableRewards { get; } = new();
        public ObservableCollection<RewardItemDto> LockedRewards { get; } = new();

        public ObservableCollection<RedeemedRewardDto> RedeemedRewards { get; } = new();

        public ICommand GoHomeCommand { get; }
        public ICommand GoAppointmentsCommand { get; }
        public ICommand GoRewardsCommand { get; }
        public ICommand GoAccountCommand { get; }
        public ICommand RedeemCommand { get; }
        public ICommand RefreshCommand { get; }

        public RewardsViewModel(SessionService session, RewardClient rewardClient, RewardCatalogClient catalogClient)
        {
            _session = session;
            _rewardClient = rewardClient;
            _catalogClient = catalogClient;

            GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await Shell.Current.GoToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await Shell.Current.GoToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));
            RedeemCommand = new Command<RewardItemDto>(async (item) => await RedeemReward(item));
            RefreshCommand = new Command(async () => await LoadRewardsAsync());

            LoadRewardsAsync();
        }



        private async Task LoadRewardsAsync()
        {
            try
            {
                IsBusy = true;

                // Load user points
                if (_session.UserId.HasValue)
                {
                    var balance = await _rewardClient.GetBalanceAsync(_session.UserId.Value);
                    Points = balance ?? 0;
                }

                // Load reward catalog
                var catalog = await _catalogClient.GetAllAsync() ?? new List<RewardItemDto>();
                AvailableRewards.Clear();
                LockedRewards.Clear();

                System.Diagnostics.Debug.WriteLine($"Loaded {catalog.Count} catalog items, User has {Points} points");

                RedeemedRewards.Clear();
                if (_session.UserId.HasValue)
                {
                    var redeemed = await _catalogClient.GetRedeemedAsync(_session.UserId.Value) ?? new List<RedeemedRewardDto>();

                    System.Diagnostics.Debug.WriteLine($"Loaded {redeemed.Count} redeemed rewards for user {_session.UserId.Value}");

                    foreach (var item in redeemed)
                    {
                        System.Diagnostics.Debug.WriteLine($"Redeemed item: Name='{item.Name}', Code='{item.Code}', RedeemedAt={item.RedeemedAt}");
                        RedeemedRewards.Add(item);
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"RedeemedRewards collection now has {RedeemedRewards.Count} items");
                }
                OnPropertyChanged(nameof(RedeemedRewards));

                if (catalog is not null)
                {
                    foreach (var reward in catalog)
                    {
                        System.Diagnostics.Debug.WriteLine($"Item: '{reward.Name}', Description: '{reward.Description}', Cost: {reward.Cost}, Points: {Points}");
                        System.Diagnostics.Debug.WriteLine($"Name empty: {string.IsNullOrEmpty(reward.Name)}, Description empty: {string.IsNullOrEmpty(reward.Description)}");
                        
                        // Skip items with empty names or descriptions
                        if (string.IsNullOrEmpty(reward.Name) || string.IsNullOrEmpty(reward.Description))
                        {
                            System.Diagnostics.Debug.WriteLine($"Skipping item with empty data: ID={reward.Id}");
                            continue;
                        }
                        
                        if (reward.Cost <= Points)
                        {
                            AvailableRewards.Add(reward);
                            System.Diagnostics.Debug.WriteLine($"Added to Available: {reward.Name}");
                        }
                        else
                        {
                            LockedRewards.Add(reward);
                            System.Diagnostics.Debug.WriteLine($"Added to Locked: {reward.Name}");
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Available rewards count: {AvailableRewards.Count}");
                System.Diagnostics.Debug.WriteLine($"Locked rewards count: {LockedRewards.Count}");
                
                // Force UI update
                OnPropertyChanged(nameof(AvailableRewards));
                OnPropertyChanged(nameof(LockedRewards));
            }
            catch (Exception ex)
            {
                // Handle error - you might want to show a message to the user
                System.Diagnostics.Debug.WriteLine($"Error loading rewards: {ex.Message}");
                
            }
            finally
            {
                IsBusy = false;
            }
        }



        private async Task RedeemReward(RewardItemDto item)
        {
            try
            {
                if (!_session.UserId.HasValue)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please log in to redeem rewards.", "OK");
                    return;
                }

                if (Points < item.Cost)
                {
                    await Application.Current.MainPage.DisplayAlert("Insufficient Points", 
                        $"You need {item.Cost} points to redeem this item. You currently have {Points} points.", "OK");
                    return;
                }

                var confirmed = await Application.Current.MainPage.DisplayAlert("Confirm Redemption", 
                    $"Are you sure you want to redeem '{item.Name}' for {item.Cost} points?", "Yes", "No");

                if (confirmed)
                {
                    var response = await _catalogClient.RedeemItemAsync(item.Id, _session.UserId.Value);

                    if (response?.Redeemed > 0)
                    {
                        // Display success message with the redemption code
                        var message = $"Successfully redeemed '{item.Name}'!\n\nYour redemption code: {response.Code}";
                        if (!string.IsNullOrEmpty(response.Code))
                        {
                            message = $"Successfully redeemed '{item.Name}'!\n\nYour redemption code:\n{response.Code}";
                        }
                        
                        await Application.Current.MainPage.DisplayAlert("Success", message, "OK");
                        
                        // Refresh the rewards list and balance
                        await LoadRewardsAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", 
                            "Failed to redeem item. Please try again.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", 
                    $"An error occurred: {ex.Message}", "OK");
            }
        }

    }

}
