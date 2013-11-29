namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class DetalleGeneralRepository : Repository<DetalleGeneral>, IDetalleGeneralRepository
    {
        public DetalleGeneralRepository(ISession session)
            : base(session)
        { 
        }
    }
}