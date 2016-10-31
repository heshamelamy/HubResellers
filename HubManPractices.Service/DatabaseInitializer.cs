using HubManPractices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Service
{
   public  class DatabaseInitializer
    {
        public static void InitializeDb()
        {
            new StoreSeedData().InitializeDatabase(new ApplicationEntities());
        }
    }
}
