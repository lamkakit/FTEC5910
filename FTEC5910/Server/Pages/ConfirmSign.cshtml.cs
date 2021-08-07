using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FTEC5910.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FTEC5910.Server.Pages
{
    [IgnoreAntiforgeryToken]

    public class ConfirmSignModel : PageModel
    {        
        [BindProperty]
        public string IdentificationCode { get; set; }

        [BindProperty]
        public string PollId { get; set; }

        public bool isError = false;
        public string errorMsg = "";
        public DataContext _db;

        public string confirmLink = "";

        public ConfirmSignModel(DataContext db) 
        {
            _db = db;
        }

        public async Task OnGetAsync(string id, string code)
        {
            if (id == null)
            {
                isError = true;
                errorMsg = "Missing id";
            }
            else 
            {
                try
                {
                    IdentificationCode = FTEC5910.Shared.Utilities.Decrypt(code);
                }
                catch (Exception ex)
                {
                    isError = true;
                    errorMsg = "Invalid code";
                    return;
                }
                try
                {
                    Guid guid;
                    if (!Guid.TryParse(id, out guid))
                    {
                        throw new Exception("Wrong ID format!");
                    }
                    var poll = _db.PollingResults.Where(a => a.RequestID == guid && a.Type.Equals("Sign")).FirstOrDefault();
                    if (poll != null && poll.Status == "Wait")
                    {
                        confirmLink = $"api/sign/confirmpoll?id={id}";
                        PollId = id;
                        return;
                    }
                    else
                    {
                        throw new Exception("Id not found or invalid status!");
                    }
                }
                catch (Exception ex)
                {
                    isError = true;
                    if (ex.InnerException != null)
                    {
                        errorMsg = $"{ex.Message} {ex.InnerException.Message}";
                    }
                    else
                    {
                        errorMsg = $"{ex.Message}";
                    }
                }
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                var uri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/api/callback/receiveSigningResult";
                HttpResponseMessage response;

                string hashCode = Shared.Utilities.ComputeSHA512(PollId);
                long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                string signature = Shared.Utilities.ComputeSHA512(hashCode + timestamp.ToString());

                byte[] byteData = Encoding.UTF8.GetBytes("{\"code\":\"D00000\",\"message\":\"SUCCESS\",\"content\":{\"businessID\":\"" + PollId + "\",\"state\":\"unesidkd\",\"hashCode\":\""+ hashCode + "\",\"timestamp\":" + timestamp .ToString() + ",\"signature\":\"" + signature + "\",\"cert\":\"sdfGSDGsdfaGDEHfjslgGQGGrGSGjljlkjwmh\"}}");

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(uri, content);
                }
                return Redirect("/SuccesseSign");

            }
            catch (Exception ex) 
            {
                return new OkObjectResult($"Fail - {ex.Message}");
            }

        }
    }
}
