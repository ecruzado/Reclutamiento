namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class NotificacionDeAprobacionRepository : Repository<NotificacionDeAprobacion>, INotificacionDeAprobacionRepository
    {
        public NotificacionDeAprobacionRepository(ISession session)
            : base(session)
        { 
        }
    }
}