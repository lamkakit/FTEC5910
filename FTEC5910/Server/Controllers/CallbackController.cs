using FTEC5910.Server.Data;
using FTEC5910.Shared.Entities.Dto;
using FTEC5910.Shared.Entities.Models;
using FTEC5910.Shared.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace FTEC5910.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly DataContext _db;

        public CallbackController(UserManager<MyIdentityUser> userManager, IConfiguration configuration, DataContext db)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            _db = db;
        }

        [HttpGet("ReceiveAuthCode")]
        public async Task<IActionResult> ReceiveAuthCode(string code)
        {
            try
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                client.DefaultRequestHeaders.Add("clientID", "edae2e2529ff46228af1e4d18c8405d1");
                client.DefaultRequestHeaders.Add("signatureMethod", "HmacSHA256");
                client.DefaultRequestHeaders.Add("timestamp", "1557048906183");
                client.DefaultRequestHeaders.Add("nonce", "e893647dc4204eb9b7b8eddd527b687c");
                client.DefaultRequestHeaders.Add("signature", "5X42Y1B7MEd8Mm%2BonwjiQz9VCZkkrntADskXsYntavU%3D");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "d6181f189d8442fbb35110a335149670");

                var uri = "https://test-eidapi-cyberport.azure-api.net/authentication/api/v1/auth/getToken?" + queryString;

                HttpResponseMessage response;

                // Request body
                byte[] byteData = Encoding.UTF8.GetBytes($"{{\"code\": \"{code}\",\"grantType\": \"authorization_code\"}}");

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(uri, content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<GetTokenResponseDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    var user = _userManager.Users.Where(a => a.IAMSmartID.Equals(result.Content.OpenID)).FirstOrDefault();
                    if (user == null)
                    {
                        return Ok(new CallBackAuthResponseDto() { IsAuthSuccessful = false, ErrorMessage = "Not linked" });
                    }
                    else 
                    {
                        var audienceList = new List<string>();
                        _jwtSettings.GetSection("ValidAudiences").Bind(audienceList);
                        var roles = await _userManager.GetRolesAsync(user);
                        var signingCredentials = JwtFunctions.GetSigningCredentials(_jwtSettings["SecurityKey"]);
                        var claims = JwtFunctions.GetClaims(user, roles, audienceList);
                        var token = JwtFunctions.GenerateToken(signingCredentials, claims, _jwtSettings["ValidIssuer"], _jwtSettings["ExpiryInMinutes"]);
 
                        return Ok(new CallBackAuthResponseDto { IsAuthSuccessful = true, Token = token, IAMSmartToken = result.Content.AccessToken });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new CallBackAuthResponseDto() { IsAuthSuccessful = false, ErrorMessage = $"{ex.Message}" });
            }
        }

        [HttpPost("ReceiveEMEInfo")]
        public async Task<IActionResult> ReceiveEMEInfo([FromBody] ReceiveEMEInfoRequest request)
        {
            try
            {
                Guid guid;
                if (!Guid.TryParse(request.Content.BusinessID, out guid))
                {
                    throw new Exception("Wrong ID format!");
                }
                var poll = _db.PollingResults.Where(a => a.RequestID == guid && a.Type.Equals("eMe")).FirstOrDefault();
                if (poll != null && poll.Status == "Wait")
                {
                    poll.Status = "OK";
                    EMeMessage output = new EMeMessage();
                    output.Email = request.Content.EmailAddress;
                    output.Mobile = request.Content.MobileNumber.SubscriberNumber;
                    poll.Message = JsonSerializer.Serialize(output);
                    _db.SaveChanges();
                    return Ok("OK");
                }
                else
                {
                    throw new Exception("Id not found or invalid status!");
                }
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
