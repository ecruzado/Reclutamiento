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
    public class PostulanteControllerTest
    {

        [TestMethod]
        public void Accion_general_retona_PostulanteGeneralViewModel()
        {
            var mockPersonaRepository = new Mock<IPersonaRepository>();
            PostulanteController postulanteController = new PostulanteController(mockPersonaRepository.Object);

            var resultado = postulanteController.General().Model;
            
            Assert.IsInstanceOfType(resultado, typeof(PostulanteGeneralViewModel));
        }

        [TestMethod]
        public void Accion_post_general_modelo_valido_retorna_RedirectToRouteResult()
        {
            var mockPersonaRepository = new Mock<IPersonaRepository>();
            PostulanteController postulanteController = new PostulanteController(mockPersonaRepository.Object);
            Persona persona = new Persona();

            var resultado = postulanteController.General(persona);

            Assert.IsInstanceOfType(resultado, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Accion_post_general_modelo_invalido_retorna_la_misma_vista()
        {
            var mockPersonaRepository = new Mock<IPersonaRepository>();
            PostulanteController postulanteController = new PostulanteController(mockPersonaRepository.Object);
            Persona persona = new Persona();
            postulanteController.ModelState.AddModelError("TipoDocumento", "modelo invalido");
            
            var resultado = postulanteController.General(persona);

            Assert.IsInstanceOfType(resultado, typeof(ViewResult));

        }
    }
}
