namespace SanPablo.Reclutador.Web.App_Start
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using NHibernate;
    using NHibernate.Cfg;
    using SanPablo.Reclutador.Web.Repository;

    public class NHibernateConfigurator
    {
        public Configuration Configure()
        {
            var configuration = OracleClientConfiguration.Oracle10
                 .ConnectionString(c =>
                    c.Is("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.24.64.82)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=WKN2)));User ID=CHSPHM2;Password=CHSPHM2;Pooling=True; Min Pool Size=3;"));
            var fluentConfiguration = Fluently.Configure()
                    .Database(configuration)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SedeRepository>());
            return fluentConfiguration.BuildConfiguration();
        }

        public ISessionFactory GetSessionFactory()
        {
            var configuration = Configure();
            return configuration.BuildSessionFactory();
        }
    }
}