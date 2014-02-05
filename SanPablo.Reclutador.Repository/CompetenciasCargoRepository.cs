namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class CompetenciasCargoRepository : Repository<CompetenciaCargo>, ICompetenciaCargoRepository
    {
        public CompetenciasCargoRepository(ISession session)
            : base(session)
        { 
        }
     }
}