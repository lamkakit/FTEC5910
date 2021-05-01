using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FTEC5910.Shared.Features;

namespace FTEC5910.Client.AuthProviders
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token)) 
            {
                Console.WriteLine($"No Token");
                return _anonymous;
            }
            

            var claims = JwtFunctions.ParseClaimsFromJwt(token);
            var exp = claims.Where(a => a.Type == "exp").FirstOrDefault();

            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Console.WriteLine($"token exp: {exp.Value} current: {currentTime}");
            if (exp == null || currentTime > Convert.ToInt64(exp.Value) )
            {
                Console.WriteLine($"token Expired");
                await _localStorage.RemoveItemAsync("authToken");
                NotifyUserLogout();
                return _anonymous;
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtFunctions.ParseClaimsFromJwt(token), "jwtAuthType")));
            }
        }



        public void NotifyUserAuthentication(string username)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
