using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SanPablo.Reclutador.Repository;
using SanPablo.Reclutador.Entity;

namespace SanPablo.Reclutador.Test.Repository
{
    [TestClass]
    public class EstudioPostulanteRepositoryTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var repository = new EstudioPostulanteRepository(NHibernateHelper.OpenSession());
            var persona = new Persona();
            persona.IdePersona = 11;

            //var lista = repository.GetPaging("IdeEstudiosPostulante", true, 0, 10);
            var entidad = new EstudioPostulante();
            entidad.IndicadorActualmenteEstudiando = "1";
            entidad.NombreInstitucion = "HEY";
            entidad.Postulante = persona;
            entidad.TipoArea = "00";
            entidad.TipoEducacion = "00";
            entidad.TipoNivelAlcanzado = "00";
            entidad.TipoNombreInstitucion = "1";
            entidad.TipTipoInstitucion = "1";
            //entidad.FechaEstudioInicio = ;
            repository.Add(entidad);
        }
    }
}
