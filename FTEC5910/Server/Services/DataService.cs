using FTEC5910.Server.Data;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FTEC5910.Server.Services
{
    public class DataService
    {
        private readonly DataContext _db;
        private readonly UserManager<MyIdentityUser> _userManager;

        public DataService(DataContext db, UserManager<MyIdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            var x = await _db.Database.EnsureCreatedAsync();
            var user = await _userManager.FindByNameAsync("C002");
            if (user == null) {
                var user1 = new MyIdentityUser { UserName = "C002", Email = "a@a.com" ,FullName="Chan Tai Man", Address = "HK Road"};
                var result = await _userManager.CreateAsync(user1, "123456aA!");
                Console.WriteLine("User C002 added!");
            }
        }
    }
}
