using CampaignManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CampaignManagement.Data
{


    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<CampaignStatusSummary> CampaignStatusSummaries { get; set; }

        public DbSet<CampaignsByProduct> CampaignsByProduct { get;set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CampaignStatusSummary>().HasNoKey().ToView("vw_CampaignStatusSummary");
            modelBuilder.Entity<CampaignsByProduct>().HasNoKey().ToView("vw_CampaignsByProduct");
        }




    }

}
