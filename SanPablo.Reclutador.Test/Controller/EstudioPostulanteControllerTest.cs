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
    public class EstudioPostulanteControllerTest
    {

        [TestMethod]
        public void Accion_general_retona_EstudioPostulanteGeneralViewModel()
        {
            var mockEstudioPostulanteRepository = new Mock<IEstudioPostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            var mockPostulanteRepository = new Mock<IPostulanteRepository>();
            EstudioPostulanteController estudioPostulanteController = new EstudioPostulanteController(mockEstudioPostulanteRepository.Object, mockDetalleGeneralRepository.Object, mockPostulanteRepository.Object);

            var resultado = estudioPostulanteController.Edit("0").Model;

            Assert.IsInstanceOfType(resultado, typeof(EstudioPostulanteGeneralViewModel));
        }

        [TestMethod]
        public void Accion_post_general_modelo_valido_retorna_RedirectToRouteResult()
        {
            var mockEstudioPostulanteRepository = new Mock<IEstudioPostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            var mockPostulanteRepository = new Mock<IPostulanteRepository>();
            EstudioPostulanteController estudioPostulanteController = new EstudioPostulanteController(mockEstudioPostulanteRepository.Object, mockDetalleGeneralRepository.Object, mockPostulanteRepository.Object);
            EstudioPostulante estudioPostulante = new EstudioPostulante();

            var resultado = estudioPostulanteController.Edit(estudioPostulante);

            Assert.IsInstanceOfType(resultado, typeof(EstudioPostulanteGeneralViewModel));
        }

        [TestMethod]
        public void Accion_post_general_modelo_invalido_retorna_la_misma_vista()
        {
            var mockEstudioPostulanteRepository = new Mock<IEstudioPostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            var mockPostulanteRepository = new Mock<IPostulanteRepository>();
            EstudioPostulanteController estudioPostulanteController = new EstudioPostulanteController(mockEstudioPostulanteRepository.Object, mockDetalleGeneralRepository.Object, mockPostulanteRepository.Object);
            EstudioPostulante estudioPostulante = new EstudioPostulante();
            estudioPostulanteController.ModelState.AddModelError("IdeEstudiosPostulante", "modelo invalido");
            var resultado = estudioPostulanteController.Edit(estudioPostulante);

            Assert.IsInstanceOfType(resultado, typeof(EstudioPostulanteGeneralViewModel));
        }
    }
}
