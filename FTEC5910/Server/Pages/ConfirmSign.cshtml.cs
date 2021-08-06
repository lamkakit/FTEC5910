using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return new OkObjectResult($"OK");
        }
    }
}
