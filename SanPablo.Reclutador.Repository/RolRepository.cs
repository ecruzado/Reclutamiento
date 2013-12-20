namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class RolRepository : Repository<Rol>, IRolRepository
    {
        public RolRepository(ISession session)
            : base(session)
        { 
        }
    }
}