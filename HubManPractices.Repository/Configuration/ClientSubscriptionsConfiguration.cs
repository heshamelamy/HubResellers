using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Configuration
{
    public class ClientSubscriptionsConfiguration : EntityTypeConfiguration<ClientSubscriptions>
    {
        public ClientSubscriptionsConfiguration()
        {
            ToTable("ClientSubscriptions");
            Property(lu => lu.ClientID).HasColumnOrder(0);
            Property(lu => lu.SubscriptionID).HasColumnOrder(1);
            HasKey(e => new { e.ClientID, e.SubscriptionID });
        }
    }
        
}
