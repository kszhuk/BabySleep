using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BabySleepWeb.Models
{
    public class InputLoginModel
    {
        [Required(ErrorMessage ="Empty Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Emailll")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Empty Password")]
        [DisplayName("Pwd")]
        public string Password { get; set; }
    }
}
