﻿namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Repository.Interface;
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

        public IList<TEntity> All()
        {
            return _session.QueryOver<TEntity>().List();
        }

        public int Add(TEntity entity)
        {
            int resultado = (int)_session.Save(entity);
            _session.Flush();
            return resultado;
        }

        public void Remove(TEntity entity)
        {
            _session.Delete(entity);
            _session.Flush();
        }

        public void Update(TEntity entity)
        {
            _session.Update(entity);
            _session.Flush();
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> condition)
        {
            _session.Clear();
            return _session.QueryOver<TEntity>()
                    .Where(condition)
                    .SingleOrDefault();
        }

        public IList<TEntity> GetBy(Expression<Func<TEntity, bool>> condition)
        {
            return _session.QueryOver<TEntity>()
                    .Where(condition)
                    .List();
        }

        public IList<TEntity> GetPaging(string sortField, bool ascending, int pageIndex, int pageSize)
        {
            return GetPaging(sortField, ascending, pageIndex, pageSize, null);
        }

        public IList<TEntity> GetPaging(string sortField, bool ascending, int pageIndex, int pageSize, DetachedCriteria where)
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

        public int CountBy(DetachedCriteria where)
        {
            DetachedCriteria whereCount = NHibernate.CriteriaTransformer.Clone(where);
            return whereCount.GetExecutableCriteria(_session)
                    .SetProjection(Projections.RowCount())
                    .FutureValue<int>()
                    .Value;
        }

        public int CountByExpress(Expression<Func<TEntity, bool>> condition)
        {
            return _session.QueryOver<TEntity>()
                    .Where(condition)
                    .RowCount();

        }

        public int getMaxValue(string campo, Expression<Func<TEntity, bool>> condition)
        {
            return _session.QueryOver<TEntity>()
                   .Where(condition)
                   .Select(Projections.Max(campo))
                   .SingleOrDefault<int>();
        }

        public virtual ResultadoQuery<TEntity> GetPagingBySql(string sortField, bool ascending, int pageIndex, int pageSize, string where)
        {
            return null;
        }

    }

}