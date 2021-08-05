using FTEC5910.Client.AuthProviders;
using FTEC5910.Shared.Entities.Dto;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace FTEC5910.Client.Data.Services
{
    public class AccountsService
    {
        private readonly HttpClient _http;
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AccountsService(HttpClient http, Blazored.LocalStorage.ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider) 
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication) 
        {
            try
            {
                var authResult = await _http.PostAsJsonAsync("/api/accounts/login", userForAuthentication);
                var authContent = await authResult.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                await _localStorage.SetItemAsync("authToken", result.Token);
                ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

                return result;
            }
            catch (Exception ex)
            {
                return new AuthResponseDto() { IsAuthSuccessful = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<GetUserResponseDto> GetUserInfo()
        {
            try
            {
                var result = await _http.GetAsync("/api/accounts/UserInfo");
                var content = await result.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<GetUserResponseDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return user;
            }
            catch (Exception ex) 
            {
                return new GetUserResponseDto() { FullName = ex.Message };
            }
        }
        public async Task<string> SetiAMSmartLoginToken(string message)
        {
            LoginMessage loginMessage = JsonSerializer.Deserialize<LoginMessage>(message, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (loginMessage != null && loginMessage.IsAuthSuccessful)
            {
                await _localStorage.SetItemAsync("iAMSmartAuthToken", loginMessage.IAMSmartToken);
                await _localStorage.SetItemAsync("authToken", loginMessage.Token);
                ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(loginMessage.Token);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginMessage.Token);
                return "";
            }
            else 
            {
                return loginMessage != null ? loginMessage.ErrorMessage != null ? loginMessage.ErrorMessage : "login failed" : "login failed"  ;
            }
        }
    }
}
