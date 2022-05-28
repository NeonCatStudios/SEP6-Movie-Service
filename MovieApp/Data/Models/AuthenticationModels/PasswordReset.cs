namespace MovieApp.Data.Models.AuthenticationModels;

public class PasswordReset
{
    public String requestType { get; set; }
    public String email { get; set; }
}