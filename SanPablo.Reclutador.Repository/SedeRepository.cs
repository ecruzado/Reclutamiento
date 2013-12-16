namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class SedeRepository : Repository<Sede>, ISedeRepository
    {
        public SedeRepository(ISession session)
            : base(session)
        {
        }
    }
}