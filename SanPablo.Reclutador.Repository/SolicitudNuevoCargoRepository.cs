namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class SolicitudNuevoCargoRepository : Repository<SolicitudNuevoCargo>, ISolicitudNuevoCargoRepository
    {
        public SolicitudNuevoCargoRepository(ISession session)
            : base(session)
        {
        }
    }
}
