using FTEC5910.Server.Data;
using FTEC5910.Shared.Entities.Dto;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FTEC5910.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EMeController : PollController
    {
        public override string PollType { get { return "eMe"; } }

        public EMeController(DataContext db) : base(db)
        {
        }
    }

    //public class EMeController : ControllerBase
    //{
    //    private readonly DataContext _db;

    //    public EMeController(DataContext db)
    //    {
    //        _db = db;
    //    }

    //    [HttpGet("AddPoll")]
    //    public async Task<IActionResult> AddPoll()
    //    {
    //        try
    //        {
    //            Guid id = Guid.NewGuid();
    //            await _db.PollingResults.AddAsync(new PollingResult() { RequestID = id, Status = "Wait", Type = "eMe" });
    //            _db.SaveChanges();
    //            return Ok(new PollResponseDto() { RequestID = id, Status = "Wait" });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Ok(new PollResponseDto() { RequestID = Guid.Empty, Status = $"Fail - {ex.Message}" });
    //        }
    //    }

    //    [HttpGet("ConfirmPoll")]
    //    public async Task<IActionResult> ConfirmPoll(string id)
    //    {
    //        Guid guid = Guid.Empty;
    //        try
    //        {
    //            guid = Guid.Parse(id);
    //            var poll = _db.PollingResults.Where(a => a.RequestID == guid && a.Type.Equals("eMe")).FirstOrDefault();
    //            if (poll == null)
    //            {
    //                return Ok(new PollResponseDto() { RequestID = guid, Status = $"Fail - ID not found" });
    //            }
    //            if (poll.Status != "Wait")
    //            {
    //                return Ok(new PollResponseDto() { RequestID = guid, Status = $"Fail - ID not in Wait status" });
    //            }
    //            poll.Status = "OK";
    //            _db.SaveChanges();
    //            return Ok(new PollResponseDto() { RequestID = guid, Status = "OK" });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Ok(new PollResponseDto() { RequestID = guid, Status = $"Fail - {ex.Message}" });
    //        }
    //    }

    //    [HttpGet("QueryPoll")]
    //    public async Task<IActionResult> QueryPoll(string id)
    //    {
    //        Guid guid = Guid.Empty;
    //        try
    //        {
    //            guid = Guid.Parse(id);
    //            var poll = _db.PollingResults.Where(a => a.RequestID == guid && a.Type.Equals("eMe")).FirstOrDefault();
    //            if (poll == null)
    //            {
    //                return Ok(new PollResponseDto() { RequestID = guid, Status = $"Fail - ID not found" });
    //            }
    //            return Ok(new PollResponseDto() { RequestID = guid, Status = poll.Status });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Ok(new PollResponseDto() { RequestID = guid, Status = $"Fail - {ex.Message}" });
    //        }
    //    }

    //    [HttpGet("ClearPoll")]
    //    public async Task<IActionResult> ClearPoll()
    //    {
    //        try
    //        {
    //            _db.PollingResults.RemoveRange(_db.PollingResults.Where(a => a.Type.Equals("eMe")));
    //            _db.SaveChanges();
    //            return Ok("ok");

    //        }
    //        catch (Exception ex)
    //        {
    //            return Ok($"error - {ex.Message}");
    //        }
    //    }
    //}
}
