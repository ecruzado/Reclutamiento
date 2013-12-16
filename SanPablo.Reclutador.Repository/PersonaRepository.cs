namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class PersonaRepository : Repository<Persona>, IPersonaRepository
    {
        public PersonaRepository(ISession session)
            : base(session)
        {
        }
    }
}
