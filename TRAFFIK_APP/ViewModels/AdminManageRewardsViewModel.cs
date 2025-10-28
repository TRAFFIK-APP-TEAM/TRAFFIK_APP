using System.Collections.ObjectModel;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Reward;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.ViewModels
{
    public class AdminManageRewardsViewModel : BaseViewModel
    {
        private readonly RewardCatalogClient _catalogClient;
        private readonly SessionService _session;

        public ObservableCollection<RedeemedRewardDto> RedeemedRewards { get; } = new();
        
        public bool HasNoRedeemedRewards => RedeemedRewards.Count == 0;

        public ICommand RefreshCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand DeactivateCodeCommand { get; }

        public AdminManageRewardsViewModel(RewardCatalogClient catalogClient, SessionService session)
        {
            _catalogClient = catalogClient;
            _session = session;

            RefreshCommand = new Command(async () => await LoadRedeemedRewardsAsync());
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync("//AdminDashboardPage"));
            DeactivateCodeCommand = new Command<RedeemedRewardDto>(async (reward) => await DeactivateCodeAsync(reward));

            LoadRedeemedRewardsAsync();
        }

        public async Task LoadRedeemedRewardsAsync()
        {
            try
            {
                IsBusy = true;

                // Load all redeemed rewards (admin view)
                var redeemed = await _catalogClient.GetAllRedeemedAsync() ?? new List<RedeemedRewardDto>();

                RedeemedRewards.Clear();

                foreach (var item in redeemed)
                {
                    RedeemedRewards.Add(item);
                }

                OnPropertyChanged(nameof(HasNoRedeemedRewards));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading redeemed rewards: {ex.Message}");
                ErrorMessage = "Failed to load redeemed rewards. Please try again.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeactivateCodeAsync(RedeemedRewardDto reward)
        {
            try
            {
                if (string.IsNullOrEmpty(reward.Code))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Code is invalid.", "OK");
                    return;
                }

                var confirmed = await Application.Current.MainPage.DisplayAlert("Deactivate Code",
                    $"Are you sure you want to deactivate the code '{reward.Code}'?", "Yes", "No");

                if (confirmed)
                {
                    var success = await _catalogClient.MarkAsUsedAsync(reward.Code);

                    if (success)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success",
                            "Code has been deactivated successfully.", "OK");
                        
                        // Reload the list to reflect the change
                        await LoadRedeemedRewardsAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error",
                            "Failed to deactivate code. Please try again.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deactivating code: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}

