using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service.ViewModels
{
    public class OfficeSubscriptionViewModel
    {
        public Guid SubscriptionID { get; set; }
        public string SubscriptionName { get; set; }
        public int MonthlyFee { get; set; }
        public virtual ICollection<ClientSubscriptions> ClientSubscriptions { get; set; }
    }
}
