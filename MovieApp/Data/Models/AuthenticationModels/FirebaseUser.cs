namespace MovieApp.Data.Models.AuthenticationModels
{
    public class FirebaseUser
    {
        public String idToken { get; set; }
        public String email { get; set; }
        public String refreshToken { get; set; }
        public String expiresIn { get; set; }
        public String localId { get; set; }
        public bool registered { get; set; }
    }
}
