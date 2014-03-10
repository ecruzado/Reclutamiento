
namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class CompetenciaRequerimientoRepository : Repository<CompetenciaRequerimiento>, ICompetenciaRequerimientoRepository
    {
        public CompetenciaRequerimientoRepository(ISession session)
            : base(session)
        {
        }
    }

}
