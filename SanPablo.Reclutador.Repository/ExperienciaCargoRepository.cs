namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class ExperienciaCargoRepository : Repository<ExperienciaCargo>, IExperienciaCargoRepository
    {
        public ExperienciaCargoRepository(ISession session)
            : base(session)
        { 
        }
     }
}