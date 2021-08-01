using FTEC5910.Shared.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FTEC5910.Client.Data.Services
{
    public class CallbackService
    {
        private readonly HttpClient _http;

        public CallbackService(HttpClient http)
        {
            _http = http;
        }

        public async Task<CallBackAuthResponseDto> ReceiveAuthCode(string code)
        {
            try
            {
                var addPollResult = await _http.GetAsync($"/api/callback/receiveAuthCode?code={code}");
                var addPollContent = await addPollResult.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<CallBackAuthResponseDto>(addPollContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result;
            }
            catch (Exception ex)
            {
                return new CallBackAuthResponseDto() { IsAuthSuccessful = false,ErrorMessage=ex.Message };
            }
        }


    }
}
