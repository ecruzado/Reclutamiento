namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class RolOpcionRepository : Repository<RolOpcion>, IRolOpcionRepository
    {
        public RolOpcionRepository(ISession session)
            : base(session)
        { 
        }
    }
}