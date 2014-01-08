namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class UbigeoController : Controller
    {
        private IUbigeoRepository _ubigeoRepository;
        // GET: /Ubigeo/
        public UbigeoController(IUbigeoRepository ubigeoRepository)
        {
            _ubigeoRepository = ubigeoRepository;
           
        }
        public ActionResult Index()
        {
            return View();
        }

        public UbigeoGeneralViewModel iniciarUbigeos()
        {
            var ubigeoGeneralViewModel = new UbigeoGeneralViewModel();
            ubigeoGeneralViewModel.Ubigeo = new Ubigeo();
            ubigeoGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "0", Descripcion = "Seleccionar" });
            ubigeoGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "1", Descripcion = "Amazonas" });
            ubigeoGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "2", Descripcion = "Ancash" });
            ubigeoGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "3", Descripcion = "Apurimac" });
            ubigeoGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "4", Descripcion = "Arequipa" });
            ubigeoGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "5", Descripcion = "Cusco" });
            ubigeoGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "6", Descripcion = "Lima" });
           
            return ubigeoGeneralViewModel;
        }
        //Busquedas dependientes
        public int Listar(int ideUbigeo)
        {
            return ideUbigeo;
        }

    }
}
