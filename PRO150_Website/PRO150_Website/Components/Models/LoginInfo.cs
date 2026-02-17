using System.ComponentModel.DataAnnotations;

namespace PRO150_Website.Components.Models
{
    public class LoginInfo
    {
        [Required(ErrorMessage = "Email Rquried")]
        public string Email { get; set; } = null;

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; } = null;

        public bool ValadateLogin()
        {
            //to be implmented
            return true;
        }
    }
}
