namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class HorarioCargoRepository : Repository<HorarioCargo>, IHorarioCargoRepository
    {
        public HorarioCargoRepository(ISession session)
            : base(session)
        { 
        }
     }
}