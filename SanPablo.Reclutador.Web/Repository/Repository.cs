namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Web.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ISession _session;

        protected Repository(ISession session)
        {
            _session = session;
        }

        public IEnumerable<TEntity> All()
        {
            return _session.QueryOver<TEntity>().List();
        }

        public int Add(TEntity entity)
        {
            return (int)_session.Save(entity);
        }

        public void Remove(TEntity entity)
        {
            _session.Delete(entity);
        }

        public void Update(TEntity entity)
        {
            _session.Update(entity);
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> condition)
        {
            return _session.QueryOver<TEntity>()
                    .Where(condition)
                    .SingleOrDefault();
        }

        public IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> condition)
        {
            return _session.QueryOver<TEntity>()
                    .Where(condition)
                    .List();
        }


        public IEnumerable<TEntity> GetPaging(string sortField, bool ascending, int pageIndex, int pageSize)
        {
            return GetPaging(sortField, ascending, pageIndex, pageSize, null);
        }

        public IEnumerable<TEntity> GetPaging(string sortField, bool ascending, int pageIndex, int pageSize, DetachedCriteria where)
        {
            ICriteria criteria = where != null ? where.GetExecutableCriteria(_session) : _session.CreateCriteria<TEntity>();

            if (!string.IsNullOrEmpty(sortField))
            {
                if (!ascending) criteria.AddOrder(Order.Desc(sortField));
                else criteria.AddOrder(Order.Asc(sortField));
            }

            return criteria.SetFirstResult(pageSize * (pageIndex - 1))
                    .SetMaxResults(pageSize)
                    .List<TEntity>();
        }

        public int CountBy()
        {
            return _session.QueryOver<TEntity>()
                    .Select(Projections.RowCount())
                    .FutureValue<int>()
                    .Value;
        }

    }

}