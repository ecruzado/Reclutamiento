namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class ExamenRepository : Repository<Examen>, IExamenRepository
    {
        public ExamenRepository(ISession session)
            : base(session)
        { 
        }
    }
}