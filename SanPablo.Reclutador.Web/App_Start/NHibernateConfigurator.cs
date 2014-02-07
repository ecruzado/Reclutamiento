namespace SanPablo.Reclutador.Web.App_Start
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using NHibernate;
    using NHibernate.Cfg;
    using SanPablo.Reclutador.Mapping;
    using SanPablo.Reclutador.Repository;
    using System.IO;

    public class NHibernateConfigurator
    {
        public Configuration Configure()
        {
            string SchemaExportPath = Path.Combine(System.Environment.CurrentDirectory, "Mappings");
            var configuration = OracleClientConfiguration.Oracle10
                 .ConnectionString(c =>
                    c.Is(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            var fluentConfiguration = Fluently.Configure()
                    .Database(configuration)
                    .Mappings(m => m.FluentMappings
                        .AddFromAssemblyOf<SedeMap>());
                        //.ExportTo(@"D:\PROYECTOS\ReclutamientoPersonal\SanPablo.Reclutador.Web"));
            return fluentConfiguration.BuildConfiguration();
        }

        public ISessionFactory GetSessionFactory()
        {
            var configuration = Configure();
            return configuration.BuildSessionFactory();
        }
    }
}