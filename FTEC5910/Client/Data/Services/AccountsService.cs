using FTEC5910.Shared.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace FTEC5910.Client.Data.Services
{
    public class AccountsService
    {
        private readonly HttpClient _http;
        public AccountsService(HttpClient http) 
        {
            _http = http;
        }

        public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication) 
        {
            try
            {
                var authResult = await _http.PostAsJsonAsync("/api/accounts/login", userForAuthentication);
                var authContent = await authResult.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
    }
}
