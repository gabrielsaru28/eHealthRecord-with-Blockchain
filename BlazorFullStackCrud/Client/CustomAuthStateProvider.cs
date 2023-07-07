using Blazored.LocalStorage;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorFullStackCrud.Client
{
    /* 
     * This class will be used to get the current authentication state of the user. 
     */
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        //  we check if the user is stored in the local storage.
        // If the user is stored in the local storage, we will return an authenticated user.
        // If the user is not stored in the local storage, we will return an unauthenticated user.

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        // This will return an authentication state
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE2ODgyMjEzNjksImV4cCI6MTcxOTc1NzM2OSwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2tldEBleGFtcGxlLmNvbSIsIk5hbWUiOiJHYWJpIiwiRW1haWwiOiJnYWJpQHVwYi5ybyIsIlJvbGUiOiJNZWRpY2FsIEluc3RpdHV0aW9uIn0.WT7tqYkOJg9yJmXTgjZgo-Qjnlpo03pOysDF_vNcaR0";

            // If this is identity is empty, the user is not authorized.
            // We got an unauthenticated user.
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            //if (!string.IsNullOrEmpty(token))
            //    // If this is identity is not empty, the user is authorized, and we set the identity.
            //    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;

            //var state = new AuthenticationState(new ClaimsPrincipal());
            //string username = await _localStorage.GetItemAsStringAsync("username");

            //if (!string.IsNullOrEmpty(username))
            //{

            //    var identity = new ClaimsIdentity(new[]
            //    {
            //        new Claim(ClaimTypes.Name, username)
            //    }, "test authentication type");

            //    state = new AuthenticationState(new ClaimsPrincipal(identity));
            //}

            //NotifyAuthenticationStateChanged(Task.FromResult(state));
            //return state;
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        

    }
}