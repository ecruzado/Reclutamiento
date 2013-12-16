namespace SanPablo.Reclutador.Web.App_Start
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using NHibernate;
    using NHibernate.Cfg;
    using SanPablo.Reclutador.Repository;

    public class NHibernateConfigurator
    {
        public Configuration Configure()
        {
            var configuration = OracleClientConfiguration.Oracle10
                 .ConnectionString(c =>
                    c.Is(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            var fluentConfiguration = Fluently.Configure()
                    .Database(configuration)
                    .Mappings(m => m.FluentMappings
                        .AddFromAssemblyOf<SedeRepository>());
            return fluentConfiguration.BuildConfiguration();
        }

        public ISessionFactory GetSessionFactory()
        {
            var configuration = Configure();
            return configuration.BuildSessionFactory();
        }
    }
}