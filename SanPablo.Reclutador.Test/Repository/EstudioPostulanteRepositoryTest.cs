using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SanPablo.Reclutador.Repository;

namespace SanPablo.Reclutador.Test.Repository
{
    [TestClass]
    public class EstudioPostulanteRepositoryTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var repository = new EstudioPostulanteRepository(NHibernateHelper.OpenSession());
            var lista = repository.GetPaging("IdeEstudiosPostulante", true, 0, 10);
        }
    }
}
