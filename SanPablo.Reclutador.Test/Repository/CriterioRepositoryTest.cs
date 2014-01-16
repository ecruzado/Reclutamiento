using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SanPablo.Reclutador.Repository;
using SanPablo.Reclutador.Entity;
using NHibernate.Criterion;

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
        public void TestMethod2()
        {
            var repository = new AlternativaRepository(NHibernateHelper.OpenSession());

            DetachedCriteria where = DetachedCriteria.For<Alternativa>();
            where.Add(Expression.Eq("IdCriterio",27));

            var lista = repository.GetPaging("CodigoAlternativa", true, 0, 100, where);
        }

       /* [TestMethod]
        public void Test_DetachedCriteria()
        {
            var repository = new CriterioRepository(NHibernateHelper.OpenSession());

            DetachedCriteria where = DetachedCriteria.For<Criterio>();
            where.Add(Expression.Eq("IdeCriterio", ));

            var lista = repository.GetPaging("IdeCriterio", true, 0, 100, where);


        }*/
    }
}
