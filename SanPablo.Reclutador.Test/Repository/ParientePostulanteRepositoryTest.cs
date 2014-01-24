using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SanPablo.Reclutador.Repository;
using SanPablo.Reclutador.Entity;

namespace SanPablo.Reclutador.Test.Repository
{
    [TestClass]
    public class ParientePostulanteRepositoryTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var repository = new ParientePostulanteRepository(NHibernateHelper.OpenSession());
            var postulante = new Postulante();
            postulante.IdePostulante = 11;

            //var lista = repository.GetPaging("IdeEstudiosPostulante", true, 0, 10);
            var entidad = new ParientePostulante();
            entidad.ApellidoPaterno = "BRAVO";
            entidad.ApellidoMaterno = "VALVO";
            entidad.Postulante = postulante;
            entidad.Nombres = "SEBASTIAN";
            entidad.TipoDeVinculo = "02";
            //entidad.FechaNacimiento = ;
            //entidad.FechaEstudioInicio = ;
            repository.Add(entidad);
        }
    }
}