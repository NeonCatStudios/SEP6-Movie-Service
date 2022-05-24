
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MovieApp.Data;
using MovieApp.Data.Models.AuthenticationModels;


namespace MovieApp.Authentication
{
    public class AuthenticationProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime jsRuntime;
        private readonly AccountService accountService;

        private CachedUser cachedUser;

        public AuthenticationProvider(IJSRuntime jsRuntime, AccountService accountService)
        {
            this.jsRuntime = jsRuntime;
            this.accountService = accountService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            if (cachedUser == null)
            {
                string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    cachedUser = JsonSerializer.Deserialize<CachedUser>(userAsJson);
                    identity = SetupClaimsForUser(cachedUser);
                }
            }
            else
            {
                identity = SetupClaimsForUser(cachedUser);
            }

            ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public async Task ValidateLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
            if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");
            ClaimsIdentity identity = new ClaimsIdentity();
            try
            {
                LoginRequest loginRequest = new LoginRequest();
                loginRequest.Username = username;
                loginRequest.Password = password;
                
                CachedUser user = await accountService.Login(loginRequest);
                identity = SetupClaimsForUser(user);
                string serialisedData = JsonSerializer.Serialize(user);
                await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
                cachedUser = user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public void Logout()
        {
            cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", null);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity SetupClaimsForUser(CachedUser user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "user"));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }
        
    }
}