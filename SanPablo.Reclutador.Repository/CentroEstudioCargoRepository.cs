namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class CentroEstudioCargoRepository : Repository<CentroEstudioCargo>, ICentroEstudioCargoRepository
    {
        public CentroEstudioCargoRepository(ISession session)
            : base(session)
        { 
        }
     }
}