using System.ComponentModel.DataAnnotations;

namespace LoginIdentityCore.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        
        public string ReturnUrl { get; set; }

        public bool RemeberMe { get; set; }
    }
}
