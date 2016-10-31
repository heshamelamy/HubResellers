using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HubManPractices.Repository.Infastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private ApplicationEntities dataContext;
        private readonly IDbSet<T> dbSet;
        
        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected ApplicationEntities DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }


        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
            DbContext.Commit();
        }
        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
            DbContext.Commit();
        }
        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
            DbContext.Commit();
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
            DbContext.Commit();
        }
        public virtual T GetById(string id)
        {
            return dbSet.Find(id);
        }
        public virtual T GetById(Guid id)
        {
            return dbSet.Find(id);
        }
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

    }
}
