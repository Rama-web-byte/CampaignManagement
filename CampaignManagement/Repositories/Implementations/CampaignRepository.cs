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

        public async Task<IEnumerable<Campaign>> GetAllCampaignsAsync(int pageNo, int pageSize)
        {
            
            var campaigns = await _context.Campaigns.AsNoTracking().Include(c => c.Product)
                                                    .OrderBy(c=>c.StartDate)
                                                    .Skip((pageNo-1)*pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();
            return campaigns;
        }
        public async Task<IEnumerable<Campaign>> GetActiveCampaignsAsync(int pageNo, int pageSize)
        {
            var campaigns = await _context.Campaigns.AsNoTracking().Include(c => c.Product)
                                                    .Where(c => c.StartDate<=DateTime.Now && c.EndDate>=DateTime.Now)  // Apply filter for active campaigns
                                                    .OrderBy(c=>c.StartDate)
                                                    .Skip((pageNo-1)*pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();

            return campaigns;
        }
        public async Task<Campaign> GetCampaignByIdAsync(Guid id)
        {
            return await _context.Campaigns.Include(c => c.Product).FirstOrDefaultAsync(c => c.CampaignId == id);  // Fetch campaign by ID
        }
       
        public async Task<IEnumerable<Product>> GetActiveProducts()
        {
            var products = await _context.Products.Where(p => p.IsActive).ToListAsync();

            return products;
        }
        public async Task<bool> ProductExistAsync(Guid ProductID)
        {
            return await _context.Products.AnyAsync(p=>p.ProductId == ProductID);
        }
        public async Task CreateCampaignAsync(Campaign campaign)
       {
            await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCampaignAsync(Campaign campaign)
        {
            _context.Campaigns.Update(campaign);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCampaignAsync(Guid id)
        {
            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign != null)
            {
                _context.Campaigns.Remove(campaign);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetAllCampaignsCountAsync(bool active)
        {
            var query =  _context.Campaigns.AsNoTracking().AsQueryable();
            if(active)
            query=query.Where(c=>c.StartDate<=DateTime.Now && c.EndDate>=DateTime.Now);
           return  await query.CountAsync();
        }

     
    }

}
