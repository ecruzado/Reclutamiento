namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class OpcionRepository : Repository<Opcion>, IOpcionRepository
    {
        public OpcionRepository(ISession session)
            : base(session)
        { 
        }

    }
}