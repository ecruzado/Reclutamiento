namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class CriterioRepository : Repository<Criterio>,ICriterioRepository
    {
        public CriterioRepository(ISession session)
            : base(session)
        { 
        }
    }
}