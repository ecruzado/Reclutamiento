namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;

    public class ConfiguracionAprobacionRepository : Repository<ConfiguracionAprobacion>,IConfiguracionAprobacionRepository
    {
        public ConfiguracionAprobacionRepository(ISession session)
            : base(session)
        { 
        }
    }
}