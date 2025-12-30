using AutoMapper;
using CampaignManagement.Models;
using CampaignManagement.Repositories.Contracts;
using CampaignManagement.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace CampaignManagement.Services.Implementations
{
    public class DashboardService:IDashboardService
    {
        private readonly IDashboardRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public DashboardService(IDashboardRepository repository,IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<CampaignStatusSummary>> GetCampaignStatusSummaryAsync()
        {
            return await _repository.GetCampaignStatusSummaryAsync();
        }
        public async Task<List<CampaignsByProduct>> GetCampaignsByProductAsync()
        {
            return await _repository.GetCampaignsByProductAsync();
        }
    }
}
