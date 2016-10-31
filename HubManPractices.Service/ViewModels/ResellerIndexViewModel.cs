using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service.ViewModels
{
    public class ResellerIndexViewModel
    {
        public IEnumerable<ClientViewModel> Clients {get; set;}
        public IEnumerable<ClientViewModel> RecentClients { get; set; }
        public Dictionary<string, int> ChartData { get; set; }
        public IEnumerable<OfficeSubscriptionViewModel> AllSubs { get; set; }

    }
}
