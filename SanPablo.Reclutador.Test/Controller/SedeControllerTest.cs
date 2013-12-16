using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SanPablo.Reclutador.Web.Controllers;
using SanPablo.Reclutador.Web.Entity;
using SanPablo.Reclutador.Web.Repository;
using SanPablo.Reclutador.Web.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPablo.Reclutador.Test.Controller
{
    [TestClass]
    public class SedeControllerTest
    {
        [TestMethod]
        public void SedeController_GetIndex_GetList() 
        {
            var mockSedeRepository = new Mock<ISedeRepository>();
            SedeController sedeController = new SedeController(mockSedeRepository.Object);

            sedeController.Index();

            mockSedeRepository.Verify(x => x.All(), Times.Once);

        }
    }
}
