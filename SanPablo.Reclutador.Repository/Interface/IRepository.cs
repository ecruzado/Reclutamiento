﻿namespace SanPablo.Reclutador.Repository.Interface
{
    using NHibernate;
    using NHibernate.Criterion;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IRepository<TEntity> where TEntity : class
    {
        IList<TEntity> All();

        int Add(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        TEntity GetSingle(Expression<Func<TEntity, bool>> condition);

        IList<TEntity> GetBy(Expression<Func<TEntity, bool>> condition);

        IList<TEntity> GetPaging(string sortField, bool ascending, int pageIndex, int pageSize);
        
        /// <summary>
        /// Obtiene datos paginados especificado el campo de orden, si es asc/dsc
        /// </summary>
        /// <param name="sortField">Campo por el cual se ordena</param>
        /// <param name="ascending">Tipo de orden</param>
        /// <param name="rowNumber">Número de fila de inicio </param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        /// <param name="where">Condicion de filtrado</param>
        /// <returns></returns>
        IList<TEntity> GetPaging(string sortField, bool ascending, int pageIndex, int pageSize, DetachedCriteria where);

        int CountBy();
        int CountBy(DetachedCriteria where);

        int CountByExpress(Expression<Func<TEntity, bool>> condition);

        int getMaxValue(string campo, Expression<Func<TEntity, bool>> condition);

        ResultadoQuery<TEntity> GetPagingBySql(string sortField, bool ascending, int pageIndex, int pageSize, string where);

    }

    public class ResultadoQuery<T>
    {
        public List<T> Lista { get; set; }
        public int Count { get; set; }
    }

}