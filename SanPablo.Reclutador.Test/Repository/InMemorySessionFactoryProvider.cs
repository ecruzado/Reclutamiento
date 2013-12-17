using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SanPablo.Reclutador.Mapping;

namespace SanPablo.Reclutador.Test.Repository
{
    class InMemorySessionFactoryProvider
    {
        private static InMemorySessionFactoryProvider instance;
        public static InMemorySessionFactoryProvider Instance
        {
            get { return instance ?? (instance = new InMemorySessionFactoryProvider()); }
        }

        private ISessionFactory sessionFactory;
        private Configuration configuration;

        private InMemorySessionFactoryProvider() { }

        public void Initialize()
        {
            sessionFactory = CreateSessionFactory();
        }

        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
            .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SedeMap>())
            .ExposeConfiguration(cfg => configuration = cfg)
            .BuildSessionFactory();
        }

        public ISession OpenSession()
        {
            ISession session = sessionFactory.OpenSession();

            var export = new SchemaExport(configuration);
            export.Execute(true, true, false, session.Connection, null);

            return session;
        }

        public void Dispose()
        {
            if (sessionFactory != null)
                sessionFactory.Dispose();

            sessionFactory = null;
            configuration = null;
        }
    }
}
