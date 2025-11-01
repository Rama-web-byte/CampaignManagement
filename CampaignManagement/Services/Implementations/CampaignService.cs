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
        private const string CacheKey = "campaigns_cache";
        private readonly IMapper _mapper;
        private  static readonly List<string> _cacheKeys=new();
        public CampaignService(ICampaignRepository campaignRepository, IMemoryCache cache,IMapper mapper)
        {
            _campaignRepository = campaignRepository;
            _cache = cache;
            _mapper = mapper;
            
        }    

        public async Task<IEnumerable<CampaignsListViewModel>> GetAllCampaignsAsync(int page, int pageSize, bool active)
        {
            // Create a unique cache key based on the page, pageSize, and active filter
            string cacheKey = $"Campaigns_Page_{page}_Size_{pageSize}_Active_{active}";

            // Try to get data from cache
            if (!_cache.TryGetValue(cacheKey, out List<CampaignsListViewModel> cachedCampaigns))
            {
                // Fetch only the necessary campaigns from the database (either all or active based on 'active' parameter)
                var campaigns = active
                    ? await _campaignRepository.GetActiveCampaignsAsync() // Fetch only active campaigns
                    : await _campaignRepository.GetAllAsync(); // Fetch all campaigns

                // Calculate total count and pages based on the filtered data
                var totalCount = campaigns.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Paginate the campaigns
                var paginatedCampaigns = campaigns
                    .OrderBy(c => c.StartDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Map campaigns to view models
                var campaignViewModels = _mapper.Map<List<CampaignViewModel>>(paginatedCampaigns);

                var campaignsListViewModel = new CampaignsListViewModel
                {
                    Campaigns = campaignViewModels,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };

                // Cache the result
                cachedCampaigns = new List<CampaignsListViewModel> { campaignsListViewModel };

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _cache.Set(cacheKey, cachedCampaigns, cacheOptions);
                _cacheKeys.Add(cacheKey);
                

            }
            else
            {
               // Console.WriteLine(string.Format("Cache hit for key: {0}", cacheKey));
            }

            // Return the cached or newly fetched data

            PrintAllCacheKeys();
           
            return cachedCampaigns;
        }



        public async Task<CampaignViewModel> GetCampaignByIdAsync(Guid id)
        {
           var campaign=  await _campaignRepository.GetByIdAsync(id);
            return _mapper.Map<CampaignViewModel>(campaign);
        }
        public async Task<IEnumerable<ViewModels.Products.ProductViewModel>> GetActiveProducts()
        {
            var products = await _campaignRepository.GetActiveProducts();


            return _mapper.Map<List<ViewModels.Products.ProductViewModel>>(products);
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
            await _campaignRepository.AddAsync(newCampaign);

            // Retrieve the saved campaign (including Product info) for returning
            var savedCampaign = await _campaignRepository.GetByIdAsync(newCampaign.CampaignId);

            // Map entity to ViewModel
            var campaignViewModel = _mapper.Map<CampaignViewModel>(savedCampaign);

            return campaignViewModel;
        }


        public async Task UpdateCampaignAsync(CampaignViewModel updatecampaign)
        {
             var campaignExist=await _campaignRepository.GetByIdAsync(updatecampaign.CampaignId);
            if(campaignExist==null)
            {
                throw new KeyNotFoundException($"Campaign with ID {updatecampaign.CampaignId} doesn't exist.");
            }
            var campaignEntity = _mapper.Map<Campaign>(updatecampaign);
            await _campaignRepository.UpdateAsync(campaignEntity);
            ClearAllCache();
        }

        public async Task DeleteCampaignAsync(Guid id)
        {
            var campaignExist = await _campaignRepository.GetByIdAsync(id);
            if(campaignExist==null)
            {
                throw new KeyNotFoundException($"Campaign with ID{id} doesn't exist.");
            }
            await _campaignRepository.DeleteAsync(id);
            ClearAllCache();
        }

        private void ClearAllCache()
        {
            // Clear all cache entries at once using LINQ
           Console.WriteLine("_cacheKeys.count:{0}",$"{_cacheKeys.Count}");
            var cacheKeysToRemove = _cacheKeys.AsEnumerable();

            foreach (var key in cacheKeysToRemove)
            {
                _cache.Remove(key);
            }

            // Clear the keys list
            _cacheKeys.Clear();
        }

        public void PrintAllCacheKeys()
        {
            List<string> checkcache = new List<string>();
            checkcache.Add("Campaigns_Page_1_Size_5_Active_False");
            checkcache.Add("Campaigns_Page_2_Size_5_Active_False");
            checkcache.Add("Campaigns_Page_3_Size_5_Active_False");
            checkcache.Add("Campaigns_Page_4_Size_5_Active_False");
            foreach (var key in checkcache)
            {


                if (_cache.TryGetValue(checkcache, out List<CampaignsListViewModel> cachedCampaigns))
                {
                    Console.WriteLine("inside prinallcache{0}",key);
                }
            }
             
        }
    }

}
