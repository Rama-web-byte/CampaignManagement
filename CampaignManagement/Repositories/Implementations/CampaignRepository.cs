using CampaignManagement.Data;
using CampaignManagement.Models;
using CampaignManagement.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampaignManagement.Repositories.Implementations
{


    public class CampaignRepository : ICampaignRepository
    {
        private readonly AppDbContext _context;

        public CampaignRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Campaign>> GetAllAsync()
        {
            var campaigns = await _context.Campaigns.Include(c => c.Product).ToListAsync();
            return campaigns;
        }
        public async Task<IEnumerable<Campaign>> GetActiveCampaignsAsync()
        {
            var campaigns = await _context.Campaigns
            .Include(c => c.Product)
            .Where(c => c.IsActive)  // Apply filter for active campaigns
            .ToListAsync();

            return campaigns;
        }
        public async Task<Campaign> GetByIdAsync(Guid id)
        {
            return await _context.Campaigns.Include(c => c.Product).FirstOrDefaultAsync(c => c.CampaignId == id);  // Fetch campaign by ID
        }
       
        public async Task<IEnumerable<Product>> GetActiveProducts()
        {
            var products = await _context.Products.Where(p => p.IsActive).ToListAsync();

            return products;
        }
        public async Task AddAsync(Campaign campaign)
       {
            await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateAsync(Campaign campaign)
        //{
        //    _context.Campaigns.Update(campaign);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteAsync(Guid id)
        //{
        //    var campaign = await _context.Campaigns.FindAsync(id);
        //    if (campaign != null)
        //    {
        //        _context.Campaigns.Remove(campaign);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }

}
