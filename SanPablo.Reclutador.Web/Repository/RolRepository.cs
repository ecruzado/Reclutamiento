namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class RolRepository : Repository<Rol>, IRolRepository
    {
        public RolRepository(ISession session)
            : base(session)
        { 
        }
    }
}