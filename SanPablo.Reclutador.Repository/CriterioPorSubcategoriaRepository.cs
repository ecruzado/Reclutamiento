namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class CriterioPorSubcategoriaRepository : Repository<CriterioPorSubcategoria>, ICriterioPorSubcategoriaRepository
    {
        public CriterioPorSubcategoriaRepository(ISession session)
            : base(session)
        { 
        }
    }
}