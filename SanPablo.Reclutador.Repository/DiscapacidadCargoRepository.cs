namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class DiscapacidadCargoRepository : Repository<DiscapacidadCargo>, IDiscapacidadCargoRepository
    {
        public DiscapacidadCargoRepository(ISession session)
            : base(session)
        { 
        }
     }
}