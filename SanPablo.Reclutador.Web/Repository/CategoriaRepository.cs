namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ISession session)
            : base(session)
        { 
        }
    }
}