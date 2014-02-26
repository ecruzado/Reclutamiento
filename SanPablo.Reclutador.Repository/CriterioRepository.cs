namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System.Collections.Generic;

    public class CriterioRepository : Repository<Criterio>,ICriterioRepository
    {
        public CriterioRepository(ISession session)
            : base(session)
        { 
        }

        public IList<Criterio> ObtenerListaMarciana(string codigo)
        {
            return new List<Criterio>();
        }

        public ResultadoQuery<Criterio> GetPagingBySql(string sortField, bool ascending, int pageIndex, int pageSize, string where) 
        {
            return null;
        }
            
    }
}