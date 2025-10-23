using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {

        private readonly ILogger _logger;
        private bool _isBusy;
        private string _errorMessage = string.Empty;
        private string _statusMessage = string.Empty;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    OnPropertyChanged(nameof(IsNotBusy));
                }
            }
        }

        public bool IsNotBusy => !IsBusy;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (SetProperty(ref _errorMessage, value))
                {
                    OnPropertyChanged(nameof(HasError));
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool HasError => !string.IsNullOrEmpty(_errorMessage);
        public int? CurrentUserId { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected async Task ExecuteSafeAsync(Func<Task> action, string? status = null)
        {
            if (IsBusy) return;
            IsBusy = true;
            ErrorMessage = string.Empty;
            StatusMessage = status ?? string.Empty;

            try
            {
                await action();
            }
            catch (HttpRequestException ex)
            {
                var innerMsg = ex.InnerException != null ? $" ({ex.InnerException.Message})" : "";
                ErrorMessage = $"Connection error: {ex.Message}{innerMsg}";
                System.Diagnostics.Debug.WriteLine($"[VM ERROR] HttpRequestException: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[VM ERROR] Inner: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"[VM ERROR] Stack: {ex.StackTrace}");
            }
            catch (TaskCanceledException ex)
            {
                ErrorMessage = "Request timeout. Please try again.";
                System.Diagnostics.Debug.WriteLine($"[VM ERROR] TaskCanceledException: {ex.Message}");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"[VM ERROR] Exception Type: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"[VM ERROR] Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[VM ERROR] StackTrace: {ex.StackTrace}");
            }
            finally
            {
                IsBusy = false;
                StatusMessage = string.Empty;
            }
        }
    }
}