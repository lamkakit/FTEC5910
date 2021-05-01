using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FTEC5910.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Form> Forms { get; set; }

        public DbSet<MyIdentityUser> IdentityUser {get;set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseCosmos("Endpoint","Key",databaseName: "FTEC5910DB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyIdentityUser>().ToContainer("IdentityUser");
            modelBuilder.Entity<User>().ToContainer("Users");
            modelBuilder.Entity<User>().HasKey("UserId");
            modelBuilder.Entity<Form>().ToContainer("Forms");
            modelBuilder.Entity<Form>().HasKey("FormId");
            modelBuilder.Entity<User>().HasData(new User() { UserId = "C0000001", UserName = "Customer1" }, new User() { UserId = "C0000002", UserName = "Customer2" });
        }

    }






}
