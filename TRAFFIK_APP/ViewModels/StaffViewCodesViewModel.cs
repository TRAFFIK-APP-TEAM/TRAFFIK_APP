using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using TRAFFIK_APP.Models.Dtos.Reward;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.ViewModels
{
    public class StaffViewCodesViewModel : BaseViewModel
    {
        private readonly RewardCatalogClient _catalogClient;
        private readonly SessionService _session;

        public ObservableCollection<RedeemedRewardDto> ActiveCodes { get; } = new();
        
        public bool HasNoActiveCodes => ActiveCodes.Count == 0;

        public ICommand RefreshCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand DeactivateCodeCommand { get; }

        public StaffViewCodesViewModel(RewardCatalogClient catalogClient, SessionService session)
        {
            _catalogClient = catalogClient;
            _session = session;

            RefreshCommand = new Command(async () => await LoadActiveCodesAsync());
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            DeactivateCodeCommand = new Command<RedeemedRewardDto>(async (reward) => await DeactivateCodeAsync(reward));

            LoadActiveCodesAsync();
        }

        public async Task LoadActiveCodesAsync()
        {
            try
            {
                IsBusy = true;

                // Load all redeemed rewards
                var redeemed = await _catalogClient.GetAllRedeemedAsync() ?? new List<RedeemedRewardDto>();

                ActiveCodes.Clear();

                // Filter for active codes only (not deactivated/used)
                foreach (var item in redeemed)
                {
                    if (!item.Used) // Only show non-deactivated codes
                    {
                        ActiveCodes.Add(item);
                    }
                }

                OnPropertyChanged(nameof(HasNoActiveCodes));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading active codes: {ex.Message}");
                ErrorMessage = "Failed to load active codes. Please try again.";
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
                        await LoadActiveCodesAsync();
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

