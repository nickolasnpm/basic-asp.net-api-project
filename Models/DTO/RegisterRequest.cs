using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UdemyProject.Models.DTO
{
    public class RegisterRequest
    {

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        public List<RoleList> Roles { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        private string _emailAddress;

        [StringLength(50, MinimumLength = 1,
            ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

                if (Regex.IsMatch(value, pattern))
                {
                    _emailAddress = value;
                }
                else
                {
                    _emailAddress = "";
                }
            }
        }

        private string _password;

        [StringLength(20, MinimumLength = 8,
            ErrorMessage = "Password should have 8 to 20 characters including uppercase, lowercase, numbers and special character")]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                string pattern = "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%&_])[A-Za-z0-9!@#$%_]{8,20}$";

                if (Regex.IsMatch(value, pattern))
                {
                    _password = value;
                }
                else
                {
                    _password = "";
                }
            }
        }

        private string _confirmPassword;

        [StringLength(20, MinimumLength = 8,
            ErrorMessage = "The Password and Confirmation Password are not matched")]
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                if (value == Password)
                {
                    _confirmPassword = Password;
                }
                else
                {
                    _confirmPassword = "";
                }

            }

        }

    }
}
