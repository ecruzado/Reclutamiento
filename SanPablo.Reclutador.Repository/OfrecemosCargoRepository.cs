namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class OfrecemosCargoRepository : Repository<OfrecemosCargo>, IOfrecemosCargoRepository
    {
        public OfrecemosCargoRepository(ISession session)
            : base(session)
        { 
        }
     }
}