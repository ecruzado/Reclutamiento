namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class SubcategoriaRepository : Repository<SubCategoria>, ISubcategoriaRepository
    {
        public SubcategoriaRepository(ISession session)
            : base(session)
        { 
        }
    }
}