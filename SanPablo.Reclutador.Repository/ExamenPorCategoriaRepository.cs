namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class ExamenPorCategoriaRepository : Repository<ExamenPorCategoria>, IExamenPorCategoriaRepository
    {
        public ExamenPorCategoriaRepository(ISession session)
            : base(session)
        { 
        }
    }
}