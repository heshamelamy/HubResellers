using AutoMapper;
using HubManPractices.Models;
using HubManPractices.Repository;
using HubManPractices.Repository.Repositories;
using HubManPractices.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service
{
    public class SubscriptionsService :ISubscriptionsService
    {
          private readonly ISubscriptionsRepository SubscriptionsRepository;

        private readonly ApplicationEntities ApplicationEntity;

        public SubscriptionsService(ApplicationEntities Ae, ISubscriptionsRepository SR)
        {
            this.SubscriptionsRepository = SR;
            this.ApplicationEntity = Ae;
        }
        public IEnumerable<Models.OfficeSubscription> GetAllSubscriptions()
        {
            return SubscriptionsRepository.GetAll();
        }


        public IEnumerable<ViewModels.OfficeSubscriptionViewModel> MapToViewModel(IEnumerable<Models.OfficeSubscription> OfficeSubscriptions)
        {
            return Mapper.Map<IEnumerable<OfficeSubscription>, IEnumerable<OfficeSubscriptionViewModel>>(OfficeSubscriptions);
        }



        public OfficeSubscription GetById(Guid SubID)
        {
            return SubscriptionsRepository.GetById(SubID);
        }
    }
}
