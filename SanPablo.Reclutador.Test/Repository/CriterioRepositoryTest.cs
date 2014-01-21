using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SanPablo.Reclutador.Repository;
using SanPablo.Reclutador.Entity;
using NHibernate.Criterion;
using System.Linq;

namespace SanPablo.Reclutador.Test.Repository
{
    [TestClass]
    public class CriterioRepositoryTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var repository = new CriterioRepository(NHibernateHelper.OpenSession());
            var criterio = new Criterio();
            criterio.TipoMedicion = "02";
            criterio.TipoModo = "02";
            criterio.TipoCriterio = "02";
            criterio.Pregunta = "02";
            criterio.TipoCalificacion = "02";
            criterio.FechaCreacion = DateTime.Now;
            repository.Add(criterio);
        }

         [TestMethod]
          public void TestMethod3()
          {
           
              var repository = new AlternativaRepository(NHibernateHelper.OpenSession());
              var alternativa = repository.GetSingle(x => x.IdeAlternativa == 24);
              repository.Remove(alternativa);

          }

        /*
          [TestMethod]
          public void TestMethod2()
          {
              var repository = new AlternativaRepository(NHibernateHelper.OpenSession());

              DetachedCriteria where = DetachedCriteria.For<Alternativa>();
              where.Add(Expression.Like("IdCriterio",27));

              var lista = repository.GetPaging("IdeAlternativa", true, 0, 100, where);
          }
          */
        /*  [TestMethod]
          public void TestMethod3()
          {
           
              var repository = new AlternativaRepository(NHibernateHelper.OpenSession());
              var alternativa = repository.GetSingle(x => x.IdeAlternativa == 23);
              alternativa.NombreAlternativa = "cambio";
              repository.Update(alternativa);

          }*/

        [TestMethod]
        public void Test_DetachedCriteria()
        {
            //var repository = new CriterioRepository(NHibernateHelper.OpenSession());

            //DetachedCriteria where = DetachedCriteria.For<Criterio>();
            //where.Add(Expression.Eq("Criterio.IdeCriterio", 52));

            //var lista = repository.GetPaging("IdeCriterio", true, 0, 100, null);
            //var session = NHibernateHelper.OpenSession();
            //var logs = session.CreateQuery("Select C, D from Criterio C, DetalleGeneral D where C.TipoMedicion = D.Valor and D.IdeGeneral = 1")
            //    .List();
        }
    }
}
