using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SanPablo.Reclutador.Repository;
using SanPablo.Reclutador.Entity;

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
    }
}
