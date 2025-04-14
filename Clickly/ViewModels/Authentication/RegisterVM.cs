using System.ComponentModel.DataAnnotations;

namespace Clickly.ViewModels.Authentication
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage ="First name must contain only letters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain only letters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "The confirm password doesn't match password.")]
        public string ConfirmPassword { get; set; }

    }
}
