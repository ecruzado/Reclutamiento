namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class ExamenRepository : Repository<Examen>, IExamenRepository
    {
        public ExamenRepository(ISession session)
            : base(session)
        { 
        }
    }
}