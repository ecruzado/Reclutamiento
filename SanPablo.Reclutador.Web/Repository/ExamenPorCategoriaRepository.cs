namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class ExamenPorCategoriaRepository : Repository<ExamenPorCategoria>, IExamenPorCategoriaRepository
    {
        public ExamenPorCategoriaRepository(ISession session)
            : base(session)
        { 
        }
    }
}