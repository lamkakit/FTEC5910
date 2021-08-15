using FTEC5910.Server.Data;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FTEC5910.Server.Services
{
    public class DataService
    {
        private readonly DataContext _db;
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataService(DataContext db, UserManager<MyIdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeData()
        {
            var x = await _db.Database.EnsureCreatedAsync();
            if (x)
                Console.WriteLine("EnsureCreated OK");

            //try
            //{
            //    var user2 = await _userManager.FindByNameAsync("C002");
            //    await _userManager.AddToRoleAsync(user2, "Administrator");
            //}
            //catch (Exception ex) 
            //{
            //    Debug.WriteLine(ex);
            //    Console.WriteLine(ex);
            //}

            var user = await _userManager.FindByNameAsync("C002");
            MyIdentityUser user1;
            if (user == null) {
                user1 = new MyIdentityUser { UserName = "C002", Email = "a@a.com" ,FullName="Chan Tai Man", Address = "Rm 1, Flat B, 12/F, Smart Building, Smart Road, Central, Hong Kong", IAMSmartID= "liR14%2BvX%2F5hSum5uf4ERczu0KcDnIJA5BM7FoM1ag9c%3D"};
                var result = await _userManager.CreateAsync(user1, "123456aA!");
                if (result.Succeeded)
                    Console.WriteLine("User C002 added!");
                else
                    Console.WriteLine($"User C002 cannot be added! {result.Errors}");
                //await _userManager.AddToRoleAsync(user1, "Administrator");
                try
                {
                    var a = await _userManager.GetRolesAsync(user1);
                    await _userManager.AddToRoleAsync(user1, "User");
                }
                catch (Exception ex)
                {
                    await _userManager.DeleteAsync(user1);
                    Debug.WriteLine(ex);
                    Console.WriteLine(ex);
                }
            }

            user = await _userManager.FindByNameAsync("Admin");
            if (user == null)
            {
                user1 = new MyIdentityUser { UserName = "Admin", Email = "a@a.com", FullName = "System Admin", Address = "Hong Kong", IAMSmartID = "" };
                var result = await _userManager.CreateAsync(user1, "123456aA!");
                if (result.Succeeded)
                    Console.WriteLine("User Admin added!");
                else
                    Console.WriteLine($"User Admin cannot be added! {result.Errors}");
                //await _userManager.AddToRoleAsync(user1, "Administrator");
                try
                {
                    var a = await _userManager.GetRolesAsync(user1);
                    await _userManager.AddToRoleAsync(user1, "Administrator");
                }
                catch (Exception ex)
                {
                    await _userManager.DeleteAsync(user1);
                    Debug.WriteLine(ex);
                    Console.WriteLine(ex);
                }
            }

        }
    }
}
