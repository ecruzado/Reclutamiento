namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class CriterioPorSubcategoriaRepository : Repository<CriterioPorSubcategoria>, ICriterioPorSubcategoriaRepository
    {
        public CriterioPorSubcategoriaRepository(ISession session)
            : base(session)
        { 
        }
    }
}