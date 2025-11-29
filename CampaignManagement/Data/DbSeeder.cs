using CampaignManagement.Models;
using System.Linq;
using System;

namespace CampaignManagement.Data
{
    public static class DbSeeder
    {
        public static void SeedUsers(AppDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (!dbContext.Users.Any())
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = UserRole.Admin
                };

                var analyst = new User
                {
                   
                    UserName = "analyst",
                    Email = "analyst@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Analyst@123")
                
                };

                var campaignOwner = new User
                {
                    UserName = "owner",
                    Email = "owner@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Owner@123"),
                    Role = UserRole.CampaignOwner
                };
                dbContext.Users.AddRange(admin, analyst, campaignOwner);
                dbContext.SaveChanges();

            }
        }
    }
}