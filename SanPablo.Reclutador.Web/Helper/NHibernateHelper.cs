using NHibernate;
using SanPablo.Reclutador.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Helper
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static ISessionFactory SessionFactory
        {

            get
            {

                if (_sessionFactory == null)
                {

                    var configuration = new NHibernateConfigurator();

                    configuration.Configure();

                    //configuration.AddAssembly(typeof(Sede).Assembly);

                    //_sessionFactory = configuration.BuildSessionFactory();
                    _sessionFactory = configuration.GetSessionFactory();

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