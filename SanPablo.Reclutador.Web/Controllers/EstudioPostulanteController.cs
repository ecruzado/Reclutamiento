namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class EstudioPostulanteController : Controller
    {
        private IEstudioPostulanteRepository _estudioPostulanteRepository;
        private IGeneralRepository _generalRepository;
               
        public EstudioPostulanteController(IEstudioPostulanteRepository estudioPostulanteRepository)
        {
            _estudioPostulanteRepository = estudioPostulanteRepository;
            
        }

        public EstudioPostulanteGeneralViewModel inicializarEstudio()
        {
            var estudioPostulanteGeneralViewModel = new EstudioPostulanteGeneralViewModel();
            estudioPostulanteGeneralViewModel.Estudio = new EstudioPostulante();
            estudioPostulanteGeneralViewModel.TipoInstituciones = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "01", Descripcion = "Universidad" });
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "02", Descripcion = "Instituto" });
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "03", Descripcion = "Colegio" });

            /*var listaInstituciones = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.Instituciones = new List<ItemTabla>();
            var recuperarInstituciones = _generalRepository.GetBy(x => x.CodigoGeneral);
            foreach (var data in recuperarInstituciones)
            {
                listaInstituciones.Add(new ItemTabla
                {
                    Codigo = data.CodigoGeneral,
                    Descripcion = data.NombreGeneral
                });
            }
            estudioPostulanteGeneralViewModel.Instituciones = listaInstituciones;*/

            estudioPostulanteGeneralViewModel.AreasEstudio = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "01", Descripcion = "Medicina General" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "02", Descripcion = "Ing. Informática y de Sistemas" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "03", Descripcion = "Enfemeria" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "04", Descripcion = "Medicina Pediatrica" });

            estudioPostulanteGeneralViewModel.Educacion = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "01", Descripcion = "Secundaria" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "02", Descripcion = "Tecnica" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "03", Descripcion = "Universitaria" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "04", Descripcion = "Maestria" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "05", Descripcion = "Doctorado" });

            estudioPostulanteGeneralViewModel.NivelesAlcanzados = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "01", Descripcion = "Incompleta" });
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "02", Descripcion = "Completa" });
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "03", Descripcion = "En curso" });

            return estudioPostulanteGeneralViewModel;
        }
        public ViewResult Estudios()
        {
            var estudioGeneralViewModel = inicializarEstudio();
            return View(estudioGeneralViewModel);
        }

        [HttpPost]
        public ActionResult Estudios([Bind(Prefix = "Estudio")]EstudioPostulante estudioPostulante)
        {
                       
            if (!ModelState.IsValid)
            {
                var estudioPostulanteModel = inicializarEstudio();
                estudioPostulanteModel.Estudio = estudioPostulante;
                return View("General",estudioPostulanteModel);
            }
            _estudioPostulanteRepository.Add(estudioPostulante);
            return RedirectToAction("Experiencia");
        }


        public ActionResult Index()
        {
            return View();
        }

    }
}
