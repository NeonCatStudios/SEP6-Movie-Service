using MovieApp.Data.Models.AuthenticationModels;

namespace MovieApp.Data;

public class AccountService
{
    public async Task<CachedUser> Login(LoginRequest loginRequest)
    {
        CachedUser user = new CachedUser();
        CachedUser.Username = "TestUser";
        CachedUser.Token = "bruh";

        return user;
    }
    public async void Register(RegisterRequest register)
    {
    
    }
}