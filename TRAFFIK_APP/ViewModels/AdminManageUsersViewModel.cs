using System.Collections.ObjectModel;
using TRAFFIK_APP.Models.Entities;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Services;
using Microsoft.Maui.Controls;

namespace TRAFFIK_APP.ViewModels
{
    public class AdminManageUsersViewModel : BaseViewModel
    {
        private readonly UserClient _userClient;
        private readonly SessionService _sessionService;

        private string _searchText = string.Empty;
        private ObservableCollection<User> _allUsers = new();
        private ObservableCollection<User> _adminUsers = new();
        private ObservableCollection<User> _customerUsers = new();

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterUsers();
            }
        }

        public ObservableCollection<User> AdminUsers
        {
            get => _adminUsers;
            set => SetProperty(ref _adminUsers, value);
        }

        public ObservableCollection<User> CustomerUsers
        {
            get => _customerUsers;
            set => SetProperty(ref _customerUsers, value);
        }

        public Command LoadUsersCommand { get; }
        public Command SearchCommand { get; }
        public Command ViewUserCommand { get; }
        public Command DeactivateUserCommand { get; }
        public Command SuspendUserCommand { get; }
        public Command GoBackCommand { get; }

        public AdminManageUsersViewModel(UserClient userClient, SessionService sessionService)
        {
            _userClient = userClient;
            _sessionService = sessionService;

            LoadUsersCommand = new Command(() => ExecuteSafeAsync(LoadUsersAsync, "Loading users..."));
            SearchCommand = new Command(FilterUsers);
            ViewUserCommand = new Command<User>(ViewUser);
            DeactivateUserCommand = new Command<User>(DeactivateUser);
            SuspendUserCommand = new Command<User>(SuspendUser);
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync("//AdminDashboardPage"));
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var users = await _userClient.GetAllAsync();
                
                if (users != null)
                {
                    _allUsers.Clear();
                    foreach (var user in users)
                    {
                        _allUsers.Add(user);
                        System.Diagnostics.Debug.WriteLine($"Loaded user: {user.FullName ?? "NULL"} - {user.Email ?? "NULL"} - Role: {user.RoleId}");
                        System.Diagnostics.Debug.WriteLine($"User properties - FullName: '{user.FullName}', Email: '{user.Email}', IsActive: {user.IsActive}");
                    }
                    
                    FilterUsers();
                    System.Diagnostics.Debug.WriteLine($"Total users loaded: {_allUsers.Count}");
                    System.Diagnostics.Debug.WriteLine($"Admin users: {AdminUsers.Count}");
                    System.Diagnostics.Debug.WriteLine($"Customer users: {CustomerUsers.Count}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No users returned from API");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading users: {ex.Message}");
                ErrorMessage = $"Failed to load users: {ex.Message}";
            }
        }

        private void FilterUsers()
        {
            var filteredUsers = _allUsers.Where(u => 
                string.IsNullOrEmpty(SearchText) ||
                u.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            // Separate users by role
            AdminUsers.Clear();
            CustomerUsers.Clear();

            foreach (var user in filteredUsers)
            {
                System.Diagnostics.Debug.WriteLine($"Filtering user: {user.FullName} - Role: {user.RoleId}");
                if (user.RoleId == 1 || user.RoleId == 2) // Admin or Staff
                {
                    AdminUsers.Add(user);
                    System.Diagnostics.Debug.WriteLine($"Added to AdminUsers: {user.FullName}");
                }
                else if (user.RoleId == 3) // Customer
                {
                    CustomerUsers.Add(user);
                    System.Diagnostics.Debug.WriteLine($"Added to CustomerUsers: {user.FullName}");
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"After filtering - AdminUsers: {AdminUsers.Count}, CustomerUsers: {CustomerUsers.Count}");
        }

        private async void ViewUser(User user)
        {
            if (user == null) return;

            // Navigate to user details page or show user info
            await Application.Current.MainPage.DisplayAlert("User Details", 
                $"Name: {user.FullName}\nEmail: {user.Email}\nRole ID: {user.RoleId}\nActive: {user.IsActive}\nCreated: {user.CreatedAt:MMM dd, yyyy}", 
                "OK");
        }

        private async void DeactivateUser(User user)
        {
            if (user == null) return;

            var result = await Application.Current.MainPage.DisplayAlert(
                "Deactivate User", 
                $"Are you sure you want to deactivate {user.FullName}?", 
                "Yes", "Cancel");

            if (result)
            {
                try
                {
                    // Update user to inactive
                    user.IsActive = false;
                    var success = await _userClient.UpdateAsync(user.Id, user);
                    
                    if (success)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "User deactivated successfully.", "OK");
                        await LoadUsersAsync(); // Refresh the list
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to deactivate user.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to deactivate user: {ex.Message}", "OK");
                }
            }
        }

        private async void SuspendUser(User user)
        {
            if (user == null) return;

            var result = await Application.Current.MainPage.DisplayAlert(
                "Suspend User", 
                $"Are you sure you want to suspend {user.FullName}?", 
                "Yes", "Cancel");

            if (result)
            {
                try
                {
                    // Delete the user account
                    var success = await _userClient.DeleteAsync(user.Id);
                    
                    if (success)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "User suspended successfully.", "OK");
                        await LoadUsersAsync(); // Refresh the list
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to suspend user.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to suspend user: {ex.Message}", "OK");
                }
            }
        }

        public string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "Administrator",
                2 => "Staff",
                3 => "Customer",
                _ => "Unknown"
            };
        }

        public string GetRoleDisplayName(int roleId)
        {
            return roleId switch
            {
                1 => "Administrator",
                2 => "Staff/Employee",
                3 => "Customer",
                _ => "Unknown Role"
            };
        }
    }
}
