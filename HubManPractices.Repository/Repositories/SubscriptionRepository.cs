using HubManPractices.Models;
using HubManPractices.Repository.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Repositories
{
    public class SubscriptionRepository : RepositoryBase<OfficeSubscription>, ISubscriptionsRepository
    {
        public SubscriptionRepository(IDbFactory dbFactory) : base(dbFactory) { }

    }
}
