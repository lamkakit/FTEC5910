using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FTEC5910.Server.Controllers;
using FTEC5910.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FTEC5910.Server.Pages
{
    [IgnoreAntiforgeryToken]

    public class ConfirmLoginModel : PageModel
    {
        public bool isError = false;
        public string errorMsg = "";
        public string script = "";

        public DataContext _db;

        public string confirmLink = "";

        [BindProperty]
        public string PollId { get; set; }

        public ConfirmLoginModel(DataContext db) 
        {
            _db = db;
        }

        public async Task OnGetAsync(string id)
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
                    Guid guid;
                    if (!Guid.TryParse(id, out guid)) 
                    {
                        throw new Exception("Wrong ID format!");
                    }
                    var poll = _db.PollingResults.Where(a => a.RequestID == guid && a.Type.Equals("Login")).FirstOrDefault();
                    if (poll != null && poll.Status == "Wait")
                    {
                        confirmLink = $"api/qr/confirmpoll?id={id}";
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
                    else {
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
                HttpResponseMessage response;
                
                var plainTextBytes = Encoding.UTF8.GetBytes(Utilities.GenerateNonce<string>(32));
                string code =Convert.ToBase64String(plainTextBytes);

                queryString["businessID"] = PollId;
                var uri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/api/callback/receiveAuthCode?code={code}&" + queryString;

                response = await client.GetAsync(uri);

                //return new OkObjectResult($"Authorized");
                return Redirect("/Success");

                //Guid guid;
                //if (!Guid.TryParse(PollId, out guid))
                //{
                //    throw new Exception("Wrong ID format!");
                //}
                //var poll = _db.PollingResults.Where(a => a.RequestID == guid && a.Type.Equals("Login")).FirstOrDefault();
                //if (poll != null && poll.Status == "Wait")
                //{
                //    poll.Status = "OK";
                //    _db.SaveChanges();
                //    return new OkObjectResult($"OK");
                //}
                //else
                //{
                //    throw new Exception("Id not found or invalid status!");
                //}
            }
            catch (Exception ex)
            {
                return new OkObjectResult($"Fail - {ex.Message}");
            }            
        }
    }
}
