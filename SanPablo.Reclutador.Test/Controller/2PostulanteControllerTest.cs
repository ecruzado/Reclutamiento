namespace SanPablo.Reclutador.Test.Controller
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Controllers;
    using SanPablo.Reclutador.Web.Models;
    using System;
    using System.Web.Mvc;

    [TestClass]
    public class otro
    {


    }
    //public class 2PostulanteControllerTest
    //{

    //    [TestMethod]
    //    public void Accion_general_retona_PostulanteGeneralViewModel()
    //    {
    //        var mockPersonaRepository = new Mock<IPostulanteRepository>();
    //        var mockEstudioPostulanteRepository = new Mock<IEstudioPostulanteRepository>();
    //        var mockUbigeoRepository = new Mock<IUbigeoRepository>();
    //        var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
    //        PostulanteController postulanteController = new PostulanteController(mockPersonaRepository.Object,mockEstudioPostulanteRepository.Object,mockUbigeoRepository.Object, mockDetalleGeneralRepository.Object);

    //        var resultado = postulanteController.General().Model;
            
    //        Assert.IsInstanceOfType(resultado, typeof(PostulanteGeneralViewModel));
    //    }

    //    [TestMethod]
    //    public void Accion_post_general_modelo_valido_retorna_RedirectToRouteResult()
    //    {
    //        var mockPersonaRepository = new Mock<IPostulanteRepository>();
    //        var mockEstudioPostulanteRepository = new Mock<IEstudioPostulanteRepository>();
    //        var mockUbigeoRepository = new Mock<IUbigeoRepository>();
    //        var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
    //        PostulanteController postulanteController = new PostulanteController(mockPersonaRepository.Object,mockEstudioPostulanteRepository.Object,mockUbigeoRepository.Object,mockDetalleGeneralRepository.Object);
    //        Postulante postulante = new Postulante();

    //        var resultado = postulanteController.General(postulante);

    //        Assert.IsInstanceOfType(resultado, typeof(RedirectToRouteResult));
    //    }

    //    [TestMethod]
    //    public void Accion_post_general_modelo_invalido_retorna_la_misma_vista()
    //    {
    //        var mockPersonaRepository = new Mock<IPostulanteRepository>();
    //        var mockEstudioPostulanteRepository = new Mock<IEstudioPostulanteRepository>();
    //        var mockUbigeoRepository = new Mock<IUbigeoRepository>();
    //        var mockDetalleGeneralRepository = new Mock<IDetalleGeneralRepository>();
    //        PostulanteController postulanteController = new PostulanteController(mockPersonaRepository.Object,mockEstudioPostulanteRepository.Object,mockUbigeoRepository.Object,mockDetalleGeneralRepository.Object);
    //        Postulante postulante = new Postulante();
    //        postulanteController.ModelState.AddModelError("TipoDocumento", "modelo invalido");
            
    //        var resultado = postulanteController.General(postulante);

    //        Assert.IsInstanceOfType(resultado, typeof(ViewResult));

    //    }
    //}
}
