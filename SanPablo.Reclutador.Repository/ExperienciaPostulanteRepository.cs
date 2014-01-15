namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class ExperienciaPostulanteRepository : Repository<ExperienciaPostulante>, IExperienciaPostulanteRepository
    {
        public ExperienciaPostulanteRepository(ISession session)
            : base(session)
        {
        }
    }
}
