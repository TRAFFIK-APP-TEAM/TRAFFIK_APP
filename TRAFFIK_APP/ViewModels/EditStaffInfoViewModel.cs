using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.User;

namespace TRAFFIK_APP.ViewModels
{
    public class EditStaffInfoViewModel : BaseViewModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public ICommand SaveCommand { get; }

        public EditStaffInfoViewModel()
        {
            SaveCommand = new Command(OnSave);
        }

        private void OnSave()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please fill in all fields.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            var updatePayload = new UserUpdateDto
            {
                Id = 3,
                FullName = "",
                Email = "",
                PhoneNumber = this.PhoneNumber,
                Password = this.Password,
                RoleId = 3,
            };

            // Leave backend submission to backend team
            ErrorMessage = string.Empty;
        }
    }
}
