namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class UsuarioSedeRepository : Repository<UsuarioSede>, IUsuarioSedeRepository
    {
        public UsuarioSedeRepository(ISession session)
            : base(session)
        { 
        }
    }
}