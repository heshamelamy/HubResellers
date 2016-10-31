using HubManPractices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Models
{
    public class Action
    {  
        public Guid ActionID { get; set; }
        public string ActionName { get; set; }
        public DateTime Date { get; set; }
        public Client Client { get; set; }
    }
}
