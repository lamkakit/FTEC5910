using FTEC5910.Server.Data.Configuration;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FTEC5910.Server.Data
{
    public class DataContext : IdentityDbContext<MyIdentityUser,IdentityRole<string>,string,IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>,IdentityRoleClaim<string>,IdentityUserToken<string>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //public DbSet<User> Users { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<PollingResult> PollingResults { get; set; }

        //public DbSet<MyIdentityUser> IdentityUser {get;set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseCosmos("Endpoint","Key",databaseName: "FTEC5910DB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.Entity<MyIdentityUser>().Property(p => p.ConcurrencyStamp).IsConcurrencyToken(false);
            modelBuilder.Entity<MyIdentityUser>().ToContainer("Auth_IdentityUser");

            modelBuilder.Entity<IdentityRole<string>>().Property(p => p.ConcurrencyStamp).IsConcurrencyToken(false);
            modelBuilder.Entity<IdentityRole<string>>().ToContainer("Auth_IdentityRole");

            modelBuilder.Entity<IdentityUserRole<string>>().ToContainer("Auth_IdentityUserRole");

            //modelBuilder.Entity<MyIdentityUser>().ToContainer("IdentityUser");
            //modelBuilder.Entity<User>().ToContainer("Users");
            //modelBuilder.Entity<User>().HasKey("UserId");
            modelBuilder.Entity<Form>().ToContainer("Forms");
            //moddelBuilder.Entity<Form>().HasNoKey();
            modelBuilder.Entity<Form>().HasKey("FormId");
            modelBuilder.Entity<PollingResult>().ToContainer("PollingResult");
            modelBuilder.Entity<PollingResult>().HasKey("RequestID");
            //modelBuilder.Entity<User>().HasData(new User() { UserId = "C0000001", UserName = "Customer1" }, new User() { UserId = "C0000002", UserName = "Customer2" });
        }

    }






}
