namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class SedeRepository : Repository<Sede>, ISedeRepository
    {
        public SedeRepository(ISession session)
            : base(session)
        {
        }
    }
}