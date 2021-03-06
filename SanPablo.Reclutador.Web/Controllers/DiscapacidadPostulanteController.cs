﻿namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using NHibernate.Criterion;

    public class DiscapacidadPostulanteController : BaseController
    {
        private IDiscapacidadPostulanteRepository _discapacidadPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IPostulanteRepository _postulanteRepository;
        public DiscapacidadPostulanteController(IDiscapacidadPostulanteRepository discapacidadPostulanteRepository, 
                                                IDetalleGeneralRepository detalleGeneralRepository,
                                                IPostulanteRepository postulanteRepository)
        {
            _discapacidadPostulanteRepository = discapacidadPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _postulanteRepository = postulanteRepository;
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Index()
        {
            var discapacidadViewModel = InicializarDiscapacidades();
            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            discapacidadViewModel.Discapacidad.Postulante = postulante;
            return View(discapacidadViewModel);
        }

        [HttpPost]
        public virtual JsonResult Listar(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<DiscapacidadPostulante>();
                where.Add(Expression.Eq("Postulante.IdePostulante", IdePostulante));


                var generic = Listar(_discapacidadPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeDiscapacidadPostulante.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoDiscapacidad,
                                item.DescripcionDiscapacidad.ToUpper(),
                              
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ViewResult Edit(string id)
        {
            var discapacidadPostulanteViewModel = InicializarDiscapacidades();
            if (id == "0")
            {
                return View(discapacidadPostulanteViewModel);
            }
            else
            {
                int ideDiscapacidadEdit = Convert.ToInt32(id);
                var discapacidadResultado = new DiscapacidadPostulante();
                discapacidadResultado = _discapacidadPostulanteRepository.GetSingle(x => x.IdeDiscapacidadPostulante == ideDiscapacidadEdit);
                discapacidadPostulanteViewModel.Discapacidad = discapacidadResultado;
                return View(discapacidadPostulanteViewModel);
            }
        }


        [HttpPost]
        public JsonResult Edit([Bind(Prefix = "Discapacidad")]DiscapacidadPostulante discapacidadPostulante)
        {
            //var result = new JsonResult();

            var objetoUsuario = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];
            string usuarioActual = objetoUsuario.CodUsuario.Length <= 15 ? objetoUsuario.CodUsuario : objetoUsuario.CodUsuario.Substring(0, 15);

            if (!ModelState.IsValid)
            {
                return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
            }
            if (discapacidadPostulante.IdeDiscapacidadPostulante == 0)
            {
                if (IdePostulante != 0)
                {
                    discapacidadPostulante.EstadoActivo = IndicadorActivo.Activo;
                    discapacidadPostulante.FechaCreacion = FechaCreacion;
                    discapacidadPostulante.UsuarioCreacion = usuarioActual;
                    var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                    postulante.agregarDiscapacidad(discapacidadPostulante);
                    _discapacidadPostulanteRepository.Add(discapacidadPostulante);
                }
                else
                {
                    return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                var discapacidadEdit = _discapacidadPostulanteRepository.GetSingle(x => x.IdeDiscapacidadPostulante == discapacidadPostulante.IdeDiscapacidadPostulante);
                discapacidadEdit.TipoDiscapacidad = discapacidadPostulante.TipoDiscapacidad;
                discapacidadEdit.DescripcionDiscapacidad = discapacidadPostulante.DescripcionDiscapacidad;
                discapacidadEdit.FechaModificacion = FechaModificacion;
                discapacidadEdit.UsuarioModificacion = usuarioActual;
                _discapacidadPostulanteRepository.Update(discapacidadEdit);
            }
            return Json(new { msj = true }, JsonRequestBehavior.DenyGet);

        }

        public DiscapacidadPostulanteGeneralViewModel InicializarDiscapacidades()
        {
            var discapacidadPostulanteGeneralViewModel = new DiscapacidadPostulanteGeneralViewModel();
            discapacidadPostulanteGeneralViewModel.Discapacidad = new DiscapacidadPostulante();

            discapacidadPostulanteGeneralViewModel.TipoDiscapacidades = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoDiscapacidad).OrderBy(x=>x.Descripcion));
            discapacidadPostulanteGeneralViewModel.TipoDiscapacidades.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });


            return discapacidadPostulanteGeneralViewModel;
        }


        [HttpPost]
        public ActionResult eliminarDiscapacidad(int ideDiscapacidad)
        {
            ActionResult result = null;

            var discapacidadEliminar = new DiscapacidadPostulante();
            discapacidadEliminar = _discapacidadPostulanteRepository.GetSingle(x => x.IdeDiscapacidadPostulante == ideDiscapacidad);
            int antes = _discapacidadPostulanteRepository.CountBy();
            _discapacidadPostulanteRepository.Remove(discapacidadEliminar);
            int despues = _discapacidadPostulanteRepository.CountBy();

            return result;
        }


       
    }
}
