using System.ComponentModel.DataAnnotations;

namespace MovieApp.Data.Models.AuthenticationModels
{
    public class LoginRequest
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        
        public bool returnSecureToken { get; set; }
    }
}
