namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class UsuarioVistaRepository : Repository<UsuarioVista>, IUsuarioVistaRepository
    {
        public UsuarioVistaRepository(ISession session)
            : base(session)
        { 
        }
    }
}
