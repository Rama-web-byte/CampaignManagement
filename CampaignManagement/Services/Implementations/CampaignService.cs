using AutoMapper;
using CampaignManagement.Models;
using CampaignManagement.Repositories.Contracts;
using CampaignManagement.Services.Contracts;
using CampaignManagement.ViewModels.Campaigns;
using CampaignManagement.ViewModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace CampaignManagement.Services.Implementations
{


    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private  static readonly List<string> _cacheKeys=new();
        public CampaignService(ICampaignRepository campaignRepository, IMemoryCache cache,IMapper mapper)
        {
            _campaignRepository = campaignRepository;
            _cache = cache;
            _mapper = mapper;
            
        }    

        public async Task<CampaignsListViewModel> GetAllCampaignsAsync(int page, int pageSize, bool active)
        {
            // Create a unique cache key based on the page, pageSize, and active filter
            string cacheKey = $"Campaigns_Page_{page}_Size_{pageSize}_Active_{active}";

            // Try to get data from cache
            if (!_cache.TryGetValue(cacheKey, out CampaignsListViewModel? cachedCampaigns))
            {
                // Fetch only the necessary campaigns from the database (either all or active based on 'active' parameter)
                var campaigns = active
                    ? await _campaignRepository.GetActiveCampaignsAsync(page, pageSize) // Fetch only active campaigns
                    : await _campaignRepository.GetAllCampaignsAsync(page, pageSize); // Fetch all campaigns

                // Calculate total count and pages based on the filtered data
                var totalCount = await _campaignRepository.GetAllCampaignsCountAsync(active);
                var totalPages = totalCount ==0?0:(int)Math.Ceiling((double)totalCount / pageSize);

               

                // Map campaigns to view models
                var campaignViewModels = _mapper.Map<List<CampaignViewModel>>(campaigns);

                cachedCampaigns = new CampaignsListViewModel
                {
                    Campaigns = campaignViewModels,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Message=totalCount==0?"No Campaigns Found":""
                };

               

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _cache.Set(cacheKey, cachedCampaigns, cacheOptions);
                _cacheKeys.Add(cacheKey);
                

            }
           
           
            return cachedCampaigns;
        }



        public async Task<CampaignViewModel?> GetCampaignByIdAsync(Guid id)
        {
           var campaign=  await _campaignRepository.GetCampaignByIdAsync(id);
            return campaign==null?null:_mapper.Map<CampaignViewModel>(campaign);
        }
        public async Task<IEnumerable<ProductViewModel>> GetActiveProducts()
        {
            var products = await _campaignRepository.GetActiveProducts();


            return _mapper.Map<List<ProductViewModel>>(products);
        }

      
        public async Task<CampaignViewModel> AddCampaignAsync(CreateCampaignViewModel campaign)
        {

            var productExist = await _campaignRepository.ProductExistAsync(campaign.ProductId);
            if (!productExist)
            {
                throw new KeyNotFoundException($"Product with ID{campaign.ProductId} does not exist.");
            }
            // Map incoming ViewModel to entity
            var newCampaign = _mapper.Map<Campaign>(campaign);

            // Optional: ensure IsActive logic is updated if not handled by AutoMapper
            newCampaign.IsActive = newCampaign.StartDate <= DateTime.UtcNow && newCampaign.EndDate >= DateTime.UtcNow;

            // Clear cache before saving new data
            ClearAllCache();

            // Add campaign to database
            await _campaignRepository.CreateCampaignAsync(newCampaign);

            // Retrieve the saved campaign (including Product info) for returning
            var savedCampaign = await _campaignRepository.GetCampaignByIdAsync(newCampaign.CampaignId);

            // Map entity to ViewModel
            var campaignViewModel = _mapper.Map<CampaignViewModel>(savedCampaign);

            return campaignViewModel;
        }


        public async Task UpdateCampaignAsync(CampaignViewModel updateCampaign)
        {
             var existingCampaign=await _campaignRepository.GetCampaignByIdAsync(updateCampaign.CampaignId);
            if(existingCampaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {updateCampaign.CampaignId} doesn't exist.");
            }

            existingCampaign.IsActive= updateCampaign.IsActive;
            existingCampaign.StartDate = updateCampaign.StartDate;
            existingCampaign.EndDate = updateCampaign.EndDate;
            existingCampaign.CampaignsName= updateCampaign.CampaignsName;

         
            await _campaignRepository.UpdateCampaignAsync(existingCampaign);
            ClearAllCache();
        }

        public async Task DeleteCampaignAsync(Guid id)
        {
            var campaignExist = await _campaignRepository.GetCampaignByIdAsync(id);
            if(campaignExist==null)
            {
                throw new KeyNotFoundException($"Campaign with ID{id} doesn't exist.");
            }
            await _campaignRepository.DeleteCampaignAsync(id);
            ClearAllCache();
        }

        private void ClearAllCache()
        {
            // Clear all cache entries at once using LINQ
     
       

            foreach (var key in _cacheKeys)
            {
                _cache.Remove(key);
            }

            // Clear the keys list
            _cacheKeys.Clear();
        }

        
    }

}
