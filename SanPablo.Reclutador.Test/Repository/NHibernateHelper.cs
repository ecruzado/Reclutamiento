using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using SanPablo.Reclutador.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPablo.Reclutador.Test.Repository
{
    class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static ISessionFactory SessionFactory
        {

            get
            {

                if (_sessionFactory == null)
                {

                    var configuration = OracleClientConfiguration.Oracle10
                         .ConnectionString(c =>
                            c.Is(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
                    
                    var fluentConfiguration = Fluently.Configure()
                            .Database(configuration)
                            .Mappings(m => m.FluentMappings
                                .AddFromAssemblyOf<SedeMap>());
                    fluentConfiguration.BuildConfiguration();

                    _sessionFactory = fluentConfiguration.BuildSessionFactory();

                }

                return _sessionFactory;

            }

        }



        public static ISession OpenSession()
        {

            return SessionFactory.OpenSession();

        }
    }
}
