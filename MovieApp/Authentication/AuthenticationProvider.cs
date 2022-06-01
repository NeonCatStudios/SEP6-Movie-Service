
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
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

        [Inject] private MovieController _movieController { get; set; }

        public AuthenticationProvider(IJSRuntime jsRuntime, AccountService accountService, MovieController movieController)
        {
            this.jsRuntime = jsRuntime;
            this.accountService = accountService;
            _movieController = movieController;
        }

        public async Task<CachedUser> GetUserFromCache()
        {
            string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
            if (!string.IsNullOrEmpty(userAsJson))
            {
                return JsonSerializer.Deserialize<CachedUser>(userAsJson);
            }

            return null;
        }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            if (cachedUser == null)
            {
                cachedUser = await GetUserFromCache();
            }
            
            if (cachedUser != null)
            {
                identity = SetupClaimsForUser(cachedUser);
            }

            ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public async Task ValidateLogin(string email, string password)
        {
            if (string.IsNullOrEmpty(email)) throw new Exception("Enter email");
            if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");
            ClaimsIdentity identity = new ClaimsIdentity();
            CachedUser user = new CachedUser();
            
            try
            {
                LoginRequest loginRequest = new LoginRequest();
                loginRequest.email = email;
                loginRequest.password = password;

                FirebaseUser firebaseUser = await accountService.Login(loginRequest);
                
                user.UserId = firebaseUser.localId;
                user.email = firebaseUser.email;
                user.Token = firebaseUser.idToken;
                identity = SetupClaimsForUser(user);
            }
            catch (Exception e)
            {
                throw new Exception("Incorrect Email/Password");
            }
            
            string serialisedData = JsonSerializer.Serialize(user);
            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
            cachedUser = user;

            List<Movie> favorites = await _movieController.GetFavListByUserId(cachedUser.UserId);

            cachedUser.favList = new FavList(favorites);

            
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

        public Boolean RegisterUser(String email, String password)
        {
            RegisterRequest request = new RegisterRequest();
            request.email = email;
            request.password = password;
            request.returnSecureToken = true;
            return accountService.Register(request);
        }

        private ClaimsIdentity SetupClaimsForUser(CachedUser user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "user"));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }

        public CachedUser getUser()
        {
            return cachedUser;
        }

        public Boolean resetPassword(String email)
        {
            return accountService.ResetPassword(email);
        }

    }
}