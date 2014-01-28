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
    public class ExperienciaPostulanteControllerTest
    {

        [TestMethod]
        public void Accion_general_retona_ExperienciaPostulanteGeneralViewModel()
        {
            var mockExperienciaPostulanteRepository = new Mock<IExperienciaPostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            var mockPostulanteRepository = new Mock<IPostulanteRepository>();
            ExperienciaPostulanteController experienciaPostulanteController = new ExperienciaPostulanteController(mockExperienciaPostulanteRepository.Object, mockDetalleGeneralRepository.Object, mockPostulanteRepository.Object);

            var resultado = experienciaPostulanteController.Edit("0").Model;

            Assert.IsInstanceOfType(resultado, typeof(ExperienciaPostulanteGeneralViewModel));
        }

        [TestMethod]
        public void Accion_post_general_modelo_valido_retorna_RedirectToRouteResult()
        {
            var mockExperienciaPostulanteRepository = new Mock<IExperienciaPostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            var mockPostulanteRepository = new Mock<IPostulanteRepository>();
            ExperienciaPostulanteController experienciaPostulanteController = new ExperienciaPostulanteController(mockExperienciaPostulanteRepository.Object, mockDetalleGeneralRepository.Object, mockPostulanteRepository.Object);
            ExperienciaPostulante experienciaPostulante = new ExperienciaPostulante();

            var resultado = experienciaPostulanteController.Edit(experienciaPostulante);

            Assert.IsInstanceOfType(resultado, typeof(PostulanteGeneralViewModel));
        }

        [TestMethod]
        public void Accion_post_general_modelo_invalido_retorna_la_misma_vista()
        {
            var mockExperienciaPostulanteRepository = new Mock<IExperienciaPostulanteRepository>();
            var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
            var mockPostulanteRepository = new Mock<IPostulanteRepository>();
            ExperienciaPostulanteController experienciaPostulanteController = new ExperienciaPostulanteController(mockExperienciaPostulanteRepository.Object, mockDetalleGeneralRepository.Object, mockPostulanteRepository.Object);
            ExperienciaPostulante experienciaPostulante = new ExperienciaPostulante();
            experienciaPostulanteController.ModelState.AddModelError("IdeExperienciaPostulante", "modelo invalido");
            var resultado = experienciaPostulanteController.Edit(experienciaPostulante);

            Assert.IsInstanceOfType(resultado, typeof(ExperienciaPostulanteGeneralViewModel));
        }
    }
}