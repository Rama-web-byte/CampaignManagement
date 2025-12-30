using CampaignManagement.Data;
using CampaignManagement.Models;
using CampaignManagement.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CampaignManagement.Repositories.Implementations
{
    public class DashboardRepository:IDashboardRepository
    {
        private readonly AppDbContext _context;
        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CampaignStatusSummary>> GetCampaignStatusSummaryAsync()
        {
            return await _context.CampaignStatusSummaries.ToListAsync();
        }

        public async Task<List<CampaignsByProduct>> GetCampaignsByProductAsync()
        {
            return await _context.CampaignsByProduct.ToListAsync();
        }

    }
}
