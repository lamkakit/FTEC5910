using FTEC5910.Server.Data;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FTEC5910.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly DataContext _db;
        public FormsController(DataContext db)
        {
            _db = db;
        }

        [HttpPost("SubmitAddressForm")]
        public async Task<IActionResult> SubmitAddressForm([FromBody] SubmitAddressFormRequest request)
        {
            try
            {               
                Form form = new();
                form.FormId = request.Data.FormID;
                form.UserId = request.Data.UserId;
                form.UserName = request.Data.UserName;
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                };
                form.Data = JsonSerializer.Serialize(request.Data, options);
                _db.Forms.Add(form);
                _db.SaveChanges();
                return Ok("OK");
            }
            catch (Exception ex) 
            {
                return Ok($"error - {ex.Message}");
            }
        }
    }
}
