using AutoMapper;
using HubManPractices.Models;

namespace HubManPractices.Service.ViewModels.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "ViewModelToDomainMappings";
            }
        }

        protected override void Configure()
        {
            CreateMap<ClientViewModel, Client>();
            CreateMap<ResellerViewModel,Reseller>();
            CreateMap<OfficeSubscriptionViewModel, OfficeSubscription>();
        }
    }
}