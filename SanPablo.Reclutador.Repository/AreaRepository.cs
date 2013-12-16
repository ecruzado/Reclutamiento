namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        public AreaRepository(ISession session)
            : base(session)
        {
        }
    }
}