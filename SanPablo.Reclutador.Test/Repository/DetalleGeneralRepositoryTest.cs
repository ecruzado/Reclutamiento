using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SanPablo.Reclutador.Repository;
using SanPablo.Reclutador.Entity;
using NHibernate.Criterion;

namespace SanPablo.Reclutador.Test.Repository
{
    [TestClass]
    public class DetalleGeneralRepositoryTest
    {
        [TestMethod]
        public void GetByTipoTabla_debe_retornarDatos()
        {
            var repository = new DetalleGeneralRepository(NHibernateHelper.OpenSession());

            var lista = repository.GetByTipoTabla(TipoTabla.TipoVia);

            Assert.IsTrue(lista.Count > 0);
        }
    }
}
