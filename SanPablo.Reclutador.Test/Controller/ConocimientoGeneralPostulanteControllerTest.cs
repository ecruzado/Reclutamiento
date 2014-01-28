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
    public class ConocimientoGeneralPostulanteControllerTest
    {

        [TestMethod]
        public void Accion_general_retona_ConocimientoGeneralPostulanteGeneralViewModel()
        {
            var mockConocimientoGeneralPostulanteRepository = new Mock<IConocimientoGeneralPostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            var mockPostulanteRepository = new Mock<IPostulanteRepository>();
            ConocimientoGeneralPostulanteController conocimientoGeneralPostulanteController = new ConocimientoGeneralPostulanteController(mockConocimientoGeneralPostulanteRepository.Object, mockDetalleGeneralRepository.Object, mockPostulanteRepository.Object);

            var resultado = conocimientoGeneralPostulanteController.Ofimatica("0").Model;

            Assert.IsInstanceOfType(resultado, typeof(ConocimientoPostulanteGeneralViewModel));
        }

        [TestMethod]
        public void Accion_post_general_modelo_valido_retorna_RedirectToRouteResult()
        {
            var mockConocimientoGeneralPostulanteRepository = new Mock<IConocimientoGeneralPostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            var mockPostulanteRepository = new Mock<IPostulanteRepository>();
            ConocimientoGeneralPostulanteController conocimientoGeneralPostulanteController = new ConocimientoGeneralPostulanteController(mockConocimientoGeneralPostulanteRepository.Object, mockDetalleGeneralRepository.Object, mockPostulanteRepository.Object);
            ConocimientoGeneralPostulante conocimientoGeneralPostulante = new ConocimientoGeneralPostulante();

            var resultado = conocimientoGeneralPostulanteController.Ofimatica(conocimientoGeneralPostulante);

            Assert.IsInstanceOfType(resultado, typeof(ConocimientoPostulanteGeneralViewModel));
        }

        [TestMethod]
        public void Accion_post_general_modelo_invalido_retorna_la_misma_vista()
        {
            var mockConocimientoGeneralPostulanteRepository = new Mock<IConocimientoGeneralPostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            var mockPostulanteRepository = new Mock<IPostulanteRepository>();
            ConocimientoGeneralPostulanteController conocimientoGeneralPostulanteController = new ConocimientoGeneralPostulanteController(mockConocimientoGeneralPostulanteRepository.Object, mockDetalleGeneralRepository.Object, mockPostulanteRepository.Object);
            ConocimientoGeneralPostulante conocimientoGeneralPostulante = new ConocimientoGeneralPostulante();
            conocimientoGeneralPostulanteController.ModelState.AddModelError("IdeConocimientoGeneralPostulante", "modelo invalido");
            var resultado = conocimientoGeneralPostulanteController.Ofimatica(conocimientoGeneralPostulante);

            Assert.IsInstanceOfType(resultado, typeof(ConocimientoPostulanteGeneralViewModel));
        }
    }
}
