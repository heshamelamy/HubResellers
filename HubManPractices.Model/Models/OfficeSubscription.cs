using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Models
{
    public class OfficeSubscription
    {
        [Key]
        public Guid SubscriptionID { get; set; }
        public string SubscriptionName { get; set; }
        public int MonthlyFee { get; set; }
        public virtual ICollection<ClientSubscriptions> ClientSubscriptions { get; set; }
    }
}
