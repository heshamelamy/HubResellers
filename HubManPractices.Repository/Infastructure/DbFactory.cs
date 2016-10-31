using HubManPractices.Repository.Infastructure;
using HubManPractices.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubManPractices.Models;

namespace HubManPractices.Repository.Configuration
{
    public class DbFactory : Disposable, IDbFactory
    {

        ApplicationEntities db;

        public ApplicationEntities Init()
        {
            return db ?? (db = new ApplicationEntities());
        }

       protected override void DisposeCore()
       {
            if (db != null)
                db.Dispose();
       }
    }
}
