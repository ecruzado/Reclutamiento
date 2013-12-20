namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class CriterioRepository : Repository<Criterio>,ICriterioRepository
    {
        public CriterioRepository(ISession session)
            : base(session)
        { 
        }
    }
}