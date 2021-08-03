using FTEC5910.Shared.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FTEC5910.Client.Data.Services
{
    public class EMeService : PollService
    {
        public override string PollType { get { return "eMe"; } }

        public EMeService(HttpClient http) : base(http)
        {
        }
    }
    //public class EMeService
    //{
    //    private readonly HttpClient _http;

    //    public EMeService(HttpClient http)
    //    {
    //        _http = http;
    //    }
    //    public async Task<PollResponseDto> AddPoll()
    //    {
    //        try
    //        {
    //            var addPollResult = await _http.GetAsync("/api/eMe/addPoll");
    //            var addPollContent = await addPollResult.Content.ReadAsStringAsync();
    //            var result = JsonSerializer.Deserialize<PollResponseDto>(addPollContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            return new PollResponseDto() { RequestID = Guid.Empty, Status = $"Fail - {ex.Message}" };
    //        }
    //    }

    //    public async Task<PollResponseDto> QueryPoll(string id)
    //    {
    //        try
    //        {
    //            var addPollResult = await _http.GetAsync($"/api/eMe/queryPoll?id={id}");
    //            var addPollContent = await addPollResult.Content.ReadAsStringAsync();
    //            var result = JsonSerializer.Deserialize<PollResponseDto>(addPollContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            return new PollResponseDto() { RequestID = Guid.Empty, Status = $"Fail - {ex.Message}" };
    //        }
    //    }

    //}
}
