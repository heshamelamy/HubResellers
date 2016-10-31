using HubManPractices.Models;
using HubManPractices.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service
{
    public interface ISubscriptionsService
    {
        IEnumerable<OfficeSubscription> GetAllSubscriptions();
        IEnumerable<OfficeSubscriptionViewModel> MapToViewModel(IEnumerable<OfficeSubscription> OfficeSubscriptions);

        OfficeSubscription GetById(Guid SubID);
    }
}
