using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Models
{
    public class Client
    {
        public Client()
        {
            ClientSubscriptions = new List<ClientSubscriptions>();
        }
        [Key]
        public Guid ClientID { get; set; }
        public Guid ResellerID { get; set; }
        public string ClientName { get; set; }
        public string ContactName { get; set; }
        public int ContactNumber { get; set; }
        public string ContactMail { get; set; }
        public string ContactTitle { get; set; }
        public string Status { get; set; }
        public int Seats { get; set; }
        public DateTime? Expiry { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsExpiryNull { get; set; }
        public string Location { get; set;}
        public DateTime CreationDate { get; set; }
        public virtual ICollection<ClientSubscriptions> ClientSubscriptions { get; set; }
        public virtual Reseller reseller { get; set; }
    }
}
