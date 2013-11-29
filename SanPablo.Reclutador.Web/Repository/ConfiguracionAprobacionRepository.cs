namespace SanPablo.Reclutador.Web.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Web.Entity;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class ConfiguracionAprobacionRepository : Repository<ConfiguracionAprobacion>,IConfiguracionAprobacionRepository
    {
        public ConfiguracionAprobacionRepository(ISession session)
            : base(session)
        { 
        }
    }
}