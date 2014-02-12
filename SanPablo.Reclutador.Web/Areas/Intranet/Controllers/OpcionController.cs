
namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using FluentValidation;
    using FluentValidation.Results;
    using Newtonsoft.Json;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;


    public class OpcionController : BaseController
    {
        

        private IRolRepository _rolRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IRolOpcionRepository _rolOpcionRepository;

        public OpcionController(IRolRepository rolRepository, IDetalleGeneralRepository detalleGeneralRepository,
                             IRolOpcionRepository rolOpcionRepository)
        {
            _rolRepository = rolRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _rolOpcionRepository = rolOpcionRepository;
        }

        /// <summary>
        /// Inicializa el Controlador
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Inicializa la carga del popup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult PopupOpcion(int id)
        {

            OpcionViewModel objOpcion = new OpcionViewModel();
            
            objOpcion = InicializarOpcionIndex();
            objOpcion.IdRoll = id;

            return View("PopupOpcion", objOpcion);

        }

        /// <summary>
        /// Inicializa la lista de valores
        /// </summary>
        /// <returns></returns>
        public OpcionViewModel InicializarOpcionIndex()
        {
            var objOpcion = new OpcionViewModel();
            objOpcion.opcion = new Opcion();

            objOpcion.TipoEstado =
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoMant));
            objOpcion.TipoEstado.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return objOpcion;
        }

    }
}
