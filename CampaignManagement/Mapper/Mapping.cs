using AutoMapper;
using CampaignManagement.Models;
using CampaignManagement.ViewModels.Campaigns;
using CampaignManagement.ViewModels.Products;
using CampaignManagement.ViewModels.Users;

namespace CampaignManagement.Mapper
{
    public class Mapping:Profile
    {
        public Mapping()
        {
        
            CreateMap<CreateCampaignViewModel, Campaign>()
            .ForMember(dest => dest.CampaignId, opt => opt.MapFrom(src => Guid.NewGuid())) // Automatically generate a new CampaignId
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));  // Default IsActive value


            CreateMap<Campaign, CampaignViewModel>()
           .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
           .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.StartDate <= DateTime.Now && src.EndDate >= DateTime.Now));

            CreateMap<List<Campaign>, CampaignsListViewModel>()
           .ForMember(dest => dest.Campaigns, opt => opt.MapFrom(src => src))
           .ForMember(dest => dest.CurrentPage, opt => opt.Ignore()) // Set defaults as necessary
           .ForMember(dest => dest.TotalPages, opt => opt.Ignore())
           .ForMember(dest => dest.PageSize, opt => opt.Ignore())
           .ForMember(dest => dest.TotalCount, opt => opt.Ignore())
           .ForMember(dest=>  dest.Message,opt=>opt.Ignore());


            CreateMap<Models.Product, ViewModels.Products.ProductViewModel>();

            CreateMap<CampaignViewModel, Campaign>()
            .ForMember(dest => dest.Product, opt => opt.Ignore()); // ignore navigation property


            CreateMap<User, UserViewModel>();
            CreateMap<User,AdminUserViewModel>();
            CreateMap<CreateUserViewModel, User>();

            CreateMap<List<User>, UserListViewModel>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.CurrentPage, opt => opt.Ignore())
            .ForMember(dest => dest.TotalPages, opt => opt.Ignore())
            .ForMember(dest => dest.PageSize, opt => opt.Ignore())
            .ForMember(dest => dest.TotalCount, opt => opt.Ignore())
            .ForMember(dest => dest.Message, opt => opt.Ignore());
            





        }

    }
}
