using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Pos
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : ParseClaimsFromJwt(token);
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        private ClaimsIdentity ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(jwt) as JwtSecurityToken;
            claims.AddRange(jsonToken?.Claims);
            return new ClaimsIdentity(claims);
        }
    }

}
