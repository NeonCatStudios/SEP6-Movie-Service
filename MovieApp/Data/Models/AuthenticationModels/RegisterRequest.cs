using System.ComponentModel.DataAnnotations;

namespace MovieApp.Data.Models.AuthenticationModels
{
    public class RegisterRequest
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required] 
        public bool returnSecureToken { get; set; }
       
    }
}
