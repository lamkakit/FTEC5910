using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FTEC5910.Server.Controllers;
using FTEC5910.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FTEC5910.Server.Pages
{
    [IgnoreAntiforgeryToken]

    public class ConfirmEMeModel : PageModel
    {
        public bool isError = false;
        public string errorMsg = "";
        public DataContext _db;

        public string confirmLink = "";


        [BindProperty]
        public string PollId { get; set; }

        public ConfirmEMeModel(DataContext db) 
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
                    var poll = _db.PollingResults.Where(a => a.RequestID == guid && a.Type.Equals("eMe")).FirstOrDefault();
                    if (poll != null && poll.Status == "Wait")
                    {
                        confirmLink = $"api/eMe/confirmpoll?id={id}";
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
                Guid guid;
                if (!Guid.TryParse(PollId, out guid))
                {
                    throw new Exception("Wrong ID format!");
                }
                var poll = _db.PollingResults.Where(a => a.RequestID == guid && a.Type.Equals("eMe")).FirstOrDefault();
                if (poll != null && poll.Status == "Wait")
                {
                    poll.Status = "OK";
                    _db.SaveChanges();
                    return new OkObjectResult($"OK");
                }
                else
                {
                    throw new Exception("Id not found or invalid status!");
                }
            }
            catch (Exception ex)
            {
                return new OkObjectResult($"Fail - {ex.Message}");
            }            
        }
    }
}
