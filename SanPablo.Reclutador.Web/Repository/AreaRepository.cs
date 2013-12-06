namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        public AreaRepository(ISession session)
            : base(session)
        {
        }
    }
}