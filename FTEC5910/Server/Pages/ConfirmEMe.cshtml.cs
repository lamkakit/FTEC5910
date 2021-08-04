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

                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                var uri =  $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/api/callback/receiveEMEInfo";
                HttpResponseMessage response;
                byte[] byteData = Encoding.UTF8.GetBytes("{\"code\":\"D00000\",\"message\":\"SUCCESS\",\"content\":{\"businessID\":\"" + PollId + "\",\"mobileNumber\":{\"CountryCode\":\"1\",\"SubscriberNumber\":\"98765432\"},\"emailAddress\":\"scn@ogcio.gov.hk\",\"residentialAddress\":{\"ChiPremisesAddress\":{\"Region\":\"香港\",\"ChiDistrict\":{\"DcDistrict\":\"WC\",\"Sub-district\":\"灣仔\"},\"BuildingName\":\"灣仔政府大樓\",\"ChiEstate\":{\"EstateName\":\"華富\",\"ChiPhase\":{\"PhaseName\":\"華清\"}},\"ChiStreet\":{\"StreetName\":\"港灣道\",\"BuildingNoFrom\":\"12\"},\"ChiBlock\":{\"BlockDescriptor\":\"座\",\"BlockNo\":\"東\"},\"Chi3dAddress\":{\"ChiFloor\":{\"FloorNum\":\"15\"},\"ChiUnit\":{\"UnitDescriptor\":\"室\",\"UnitNo\":\"A1\"}}}},\"postalAddress\":{\"PostBoxAddress\":{\"EngPostBox\":{\"PoBoxNo\":24700,\"PostOffice\":\"ABERDEEN POST OFFICE\",\"PostOfficeRegion\":\"HONG KONG\"}}}}}");

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(uri, content);
                }
                return new OkObjectResult($"Authorized");

                //Guid guid;
                //if (!Guid.TryParse(PollId, out guid))
                //{
                //    throw new Exception("Wrong ID format!");
                //}
                //var poll = _db.PollingResults.Where(a => a.RequestID == guid && a.Type.Equals("eMe")).FirstOrDefault();
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
