using AutoMapper;
using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HubManPractices.Service.ViewModels.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "DomainToViewModelMappings";
            }
        }

        protected override void Configure()
        {
            CreateMap<Client, ClientViewModel>();
            CreateMap<Reseller, ResellerViewModel>();
            CreateMap<OfficeSubscription, OfficeSubscriptionViewModel>();
        }
    }
}