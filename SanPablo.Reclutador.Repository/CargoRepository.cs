namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class CargoRepository : Repository<Cargo>, ICargoRepository
    {
        public CargoRepository(ISession session)
            : base(session)
        {
        }
    }
}
