namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class UsuarioSedeRepository : Repository<UsuarioSede>, IUsuarioSedeRepository
    {
        public UsuarioSedeRepository(ISession session)
            : base(session)
        { 
        }
    }
}