using System.Text;
using System.Text.Json.Nodes;
using MovieApp.Data.Models.AuthenticationModels;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MovieApp.Data;

public class AccountService
{
    public async Task<FirebaseUser> Login(LoginRequest loginRequest)
    {
        using var httpClient = new HttpClient();
        loginRequest.returnSecureToken = true;
        var json = JsonConvert.SerializeObject(loginRequest);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyBfpUR_8_qwVlMQCL50k2Cd6NbHv8qbOL0", data);
        
        if (response.Result.IsSuccessStatusCode)
        {
            FirebaseUser firebaseUser = JsonSerializer.Deserialize<FirebaseUser>(response.Result.Content.ReadAsStringAsync().Result);
            return firebaseUser;
        }
        else
        {
            return null;
        }
        
    }
    public Boolean Register(RegisterRequest register)
    {
        using var httpClient = new HttpClient();
      
        var json = JsonConvert.SerializeObject(register);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyBfpUR_8_qwVlMQCL50k2Cd6NbHv8qbOL0", data);

        if (response.Result.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Boolean ResetPassword(String email)
    {
        using var httpClient = new HttpClient();
        PasswordReset passwordReset = new PasswordReset();
        passwordReset.email = email;
        passwordReset.requestType = "PASSWORD_RESET";
        var json = JsonConvert.SerializeObject(passwordReset);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=AIzaSyBfpUR_8_qwVlMQCL50k2Cd6NbHv8qbOL0", data);

        if (response.Result.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}