using FTEC5910.Server.Data;
using FTEC5910.Shared;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FTEC5910.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DataContext _db;
        private readonly UserManager<MyIdentityUser> _userManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext db, UserManager<MyIdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            //_db.Users.Add(new Data.User() { UserId = "1" });
            //_db.Add(new User() { UserId = "1" });
            try
            {

                //var a = _db.Database.GetCosmosClient().GetDatabase("FTEC5910DB").GetContainer("Users");
                //await a.CreateItemAsync(new User() { id = "A" });
                //var user = _db.Users.Where(a => a.UserId == "B").FirstOrDefault();
                //if (user != null)
                //    user.UserName = DateTimeOffset.UtcNow.ToString();
                //else
                //    await _db.Users.AddAsync(new User() { UserId = "B" });
                //_db.SaveChanges();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
