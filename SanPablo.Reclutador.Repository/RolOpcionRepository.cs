namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class RolOpcionRepository : Repository<RolOpcion>, IRolOpcionRepository
    {
        public RolOpcionRepository(ISession session)
            : base(session)
        { 
        }
    }
}