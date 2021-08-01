using FTEC5910.Client.AuthProviders;
using FTEC5910.Shared.Entities.Dto;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace FTEC5910.Client.Data.Services
{
    public class CallbackService
    {
        private readonly HttpClient _http;
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public CallbackService(HttpClient http, Blazored.LocalStorage.ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<CallBackAuthResponseDto> ReceiveAuthCode(string code)
        {
            try
            {
                var receiveAuthCodeResult = await _http.GetAsync($"/api/callback/receiveAuthCode?code={code}");
                var receiveAuthCodeContent = await receiveAuthCodeResult.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<CallBackAuthResponseDto>(receiveAuthCodeContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                await _localStorage.SetItemAsync("iAMSmartAuthToken", result.IAMSmartToken);
                await _localStorage.SetItemAsync("authToken", result.Token);
                ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

                return result;
            }
            catch (Exception ex)
            {
                return new CallBackAuthResponseDto() { IsAuthSuccessful = false,ErrorMessage=ex.Message };
            }
        }


    }
}
