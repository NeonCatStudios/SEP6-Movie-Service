using System.ComponentModel.DataAnnotations;

namespace MovieApp.Data.Models.AuthenticationModels
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set;  }
    }
}
