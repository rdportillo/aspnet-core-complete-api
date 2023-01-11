using System.ComponentModel.DataAnnotations;

namespace Dev.Api.DTO
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "The {0} field is required")]
        [EmailAddress(ErrorMessage = "The {0} field has an invalid format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(100, ErrorMessage = "The {0} field have between {2} and {1} characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
