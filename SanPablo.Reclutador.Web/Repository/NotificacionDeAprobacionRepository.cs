namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class NotificacionDeAprobacionRepository : Repository<NotificacionDeAprobacion>, INotificacionDeAprobacionRepository
    {
        public NotificacionDeAprobacionRepository(ISession session)
            : base(session)
        { 
        }
    }
}