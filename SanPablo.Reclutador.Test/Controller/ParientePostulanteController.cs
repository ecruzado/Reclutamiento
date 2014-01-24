namespace SanPablo.Reclutador.Test.Controller
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Controllers;
    using SanPablo.Reclutador.Web.Models;
    using System;
    using System.Web.Mvc;

    [TestClass]
    public class ParientePostulanteControllerTest
    {

        [TestMethod]
        public void Accion_general_retona_PostulanteGeneralViewModel()
        {
            var mockParientePersonaRepository = new Mock<IParientePostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            ParientePostulanteController parientePostulanteController = new ParientePostulanteController(mockParientePersonaRepository.Object, mockDetalleGeneralRepository.Object);

            var resultado = parientePostulanteController.Edit("0").Model;

            Assert.IsInstanceOfType(resultado, typeof(ParientePostulanteGeneralViewModel));
        }

        [TestMethod]
        public void Accion_post_general_modelo_valido_retorna_RedirectToRouteResult()
        {
            var mockParientePersonaRepository = new Mock<IParientePostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            ParientePostulanteController parientePostulanteController = new ParientePostulanteController(mockParientePersonaRepository.Object, mockDetalleGeneralRepository.Object);
            ParientePostulante parientePostulante = new ParientePostulante();

            var resultado = parientePostulanteController.Edit(parientePostulante);

            Assert.IsInstanceOfType(resultado, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Accion_post_general_modelo_invalido_retorna_la_misma_vista()
        {
            var mockParientePersonaRepository = new Mock<IParientePostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            ParientePostulanteController parientePostulanteController = new ParientePostulanteController(mockParientePersonaRepository.Object, mockDetalleGeneralRepository.Object);
            ParientePostulante parientePostulante = new ParientePostulante();
            parientePostulanteController.ModelState.AddModelError("IdeParientePostulante", "modelo invalido");

            var resultado = parientePostulanteController.Edit(parientePostulante);

            Assert.IsInstanceOfType(resultado, typeof(ViewResult));

        }
    }
}

