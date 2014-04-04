

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

    [Authorize]
    public class MantenimientoGeneralesController : BaseController
    {

        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IGeneralRepository _generalRepository;
        private ISedeRepository _sedeRepository;

        public MantenimientoGeneralesController(IDetalleGeneralRepository detalleGeneralRepository,
                                                ISedeRepository sedeRepository,
                                                IGeneralRepository generalRepository)
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _sedeRepository = sedeRepository;
            _generalRepository = generalRepository;
        }

        /// <summary>
        /// inicializa la pantalla inicial de usuarios
        /// </summary>
        /// <returns></returns>

        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult Index()
        {
            MantenimientoGeneralViewModel model = new MantenimientoGeneralViewModel();

            model = InicializarMantenimiento();

            return View("Index", model);
        }

        public MantenimientoGeneralViewModel InicializarMantenimiento()
        {
            MantenimientoGeneralViewModel mantenimientoViewModel = new MantenimientoGeneralViewModel();
            mantenimientoViewModel.TablaGeneral = new General();

            mantenimientoViewModel.tipoTablas = new List<General>(_generalRepository.GetBy(x => x.IdeGeneral != 46 && x.IdeGeneral != 47 && x.IdeGeneral != 51 && x.IdeGeneral != 50
                                                                                            && x.IdeGeneral != 3 && x.IdeGeneral != 4 && x.IdeGeneral != 49 && x.IdeGeneral != 43));

            mantenimientoViewModel.tipoTablas.Insert(0, new General { IdeGeneral = 0, TipoTabla = "Seleccionar" });
            return mantenimientoViewModel;
        }

        /// <summary>
        /// Crea un nuevo Usuario
        /// </summary>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit(MantenimientoGeneralViewModel model)
        {
            DetalleGeneral detalleGeneral = model.TablaDetalleGeneral;
            detalleGeneral.Valor = model.Valor;

            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var modelDetalle = InicializarDetalleMantenimiento();
                    modelDetalle.TablaDetalleGeneral = detalleGeneral;
                    return View("Edit", modelDetalle);
                }
                if ((detalleGeneral.Valor != null) && (detalleGeneral.Descripcion != null))
                {
                    if (detalleGeneral.Accion == Accion.Nuevo)
                    {
                        detalleGeneral.EstadoActivo = IndicadorActivo.Activo;
                        detalleGeneral.FechaCreacion = FechaCreacion;

                        detalleGeneral.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                        detalleGeneral.General = new General();
                        detalleGeneral.General.IdeGeneral = IdeGeneral;
                        if (existeValor(detalleGeneral))
                        {
                            objJsonMessage.Mensaje = "Verifique que los valores ingresados no existan";
                            objJsonMessage.Resultado = false;
                            return Json(objJsonMessage);
                        }
                        else
                        {
                            int result = _detalleGeneralRepository.insertarDetalle(detalleGeneral);
                            if (result == 1)
                            {
                                objJsonMessage.Resultado = true;
                                return Json(objJsonMessage);
                            }
                            else
                            {
                                objJsonMessage.Mensaje = "Error: Intente nuevamente";
                                objJsonMessage.Resultado = false;
                                return Json(objJsonMessage);
                            }

                        }
                    }
                    else
                    {
                        if (detalleGeneral.Accion == Accion.Editar)
                        {
                            DetalleGeneral detalleEditar = _detalleGeneralRepository.GetSingle(x => x.IdeGeneral == IdeGeneral && x.Valor == detalleGeneral.Valor);

                            detalleEditar.Valor = detalleGeneral.Valor;
                            detalleEditar.Descripcion = detalleGeneral.Descripcion;

                            var usuario = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                            detalleEditar.UsuarioModificacion = usuario.Length <= 15 ? usuario : usuario.Substring(0, 15);
                            detalleEditar.FechaModificacion = FechaModificacion;
                            _detalleGeneralRepository.Update(detalleEditar);

                            objJsonMessage.Resultado = true;
                            return Json(objJsonMessage);
                        }
                        else
                        {
                            objJsonMessage.Mensaje = "No se pudo modificar el registro";
                            objJsonMessage.Resultado = false;
                            return Json(objJsonMessage);
                        }
                    }
                }
                else
                {
                    objJsonMessage.Mensaje = "Error:Seleccione un registro a modificar";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }

            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }

        }

        /// <summary>
        /// Lista las tablas generales
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaGeneral(GridTable grid)
        {
            try
            {

                DetachedCriteria where = null;
                where = DetachedCriteria.For<General>();

                if (
                    (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0")
                   )
                {

                    if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                    {

                        where.Add(Expression.Eq("IdeGeneral", Convert.ToInt32(grid.rules[1].data)));
                    }
                    if (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0")
                    {
                        where.Add(Expression.Like("Descripcion", '%' + grid.rules[2].data + '%'));
                    }
                    
                }


                //No mostrar las tablas generales criticas

                where.Add(Expression.Not(Expression.Eq("IdeGeneral", 46)));
                where.Add(Expression.Not(Expression.Eq("IdeGeneral", 47)));
                where.Add(Expression.Not(Expression.Eq("IdeGeneral", 51)));
                where.Add(Expression.Not(Expression.Eq("IdeGeneral", 3)));
                where.Add(Expression.Not(Expression.Eq("IdeGeneral", 4)));
                where.Add(Expression.Not(Expression.Eq("IdeGeneral", 49)));
                where.Add(Expression.Not(Expression.Eq("IdeGeneral", 50)));
                where.Add(Expression.Not(Expression.Eq("IdeGeneral", 43)));

                var generic = Listar(_generalRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeGeneral.ToString(),
                    cell = new string[]
                            {
                                item.TipoTabla==null?"":item.TipoTabla.ToString(),
                                item.Descripcion==null?"":item.Descripcion.ToString(),
                                item.TipoDato==null?"":item.TipoDato,
                                item.LongitudCampo==null?"":item.LongitudCampo
                                
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

        [ValidarSesion]
        public ActionResult Edit(string id)
        {
            var model = InicializarDetalleMantenimiento();
            model.IndSubDetalle = Indicador.No;
            model.TablaDetalleGeneral.Accion = Accion.Nuevo;
            model.AccionModel = Accion.Editar;

            int idGeneral = Convert.ToInt32(id);
            if (idGeneral != 0)
            {
                IdeGeneral = idGeneral;
                model.IndSubDetalle = determinarIndSubDetalle();

            }
            return View("Edit", model);
        }

        public MantenimientoGeneralViewModel InicializarDetalleMantenimiento()
        {
            MantenimientoGeneralViewModel model = new MantenimientoGeneralViewModel();
            model.TablaDetalleGeneral = new DetalleGeneral();

            return model;
        }

        public MantenimientoGeneralViewModel InicializarSubDetalleMantenimiento()
        {
            MantenimientoGeneralViewModel model = new MantenimientoGeneralViewModel();
            model.TablaSubDetalle = new DetalleGeneral();

            return model;
        }

        [HttpPost]
        public ActionResult ListaDetalle(GridTable grid)
        {
            try
            {
                DetachedCriteria where = null;
                where = DetachedCriteria.For<DetalleGeneral>();

                where.Add(Expression.Eq("IdeGeneral", IdeGeneral));
                where.Add(Expression.IsNull("Referencia"));

                var generic = Listar(_detalleGeneralRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeGeneral.ToString()+item.Valor.ToString(),
                    cell = new string[]
                        {
                            item.Valor==null?"":item.Valor.ToString(),
                            item.Descripcion==null?"":item.Descripcion.ToString(),
                            item.IndActivo == null?"":item.IndActivo
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

         
        [HttpPost]
        public ActionResult ListaSubDetalle(GridTable grid)
        {
            try
            {
                DetachedCriteria where = null;
                where = DetachedCriteria.For<DetalleGeneral>();

                where.Add(Expression.Eq("IdeGeneral", IdeGeneral));
                where.Add(Expression.Eq("Referencia",DetalleValor));

                var generic = Listar(_detalleGeneralRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeGeneral.ToString() + item.Valor.ToString(),
                    cell = new string[]
                        {
                            item.Valor==null?"":item.Valor.ToString(),
                            item.Descripcion==null?"":item.Descripcion.ToString(),
                            item.IndActivo == null?"":item.IndActivo
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



        [ValidarSesion]
        public ActionResult SubDetalle(string id)
        {
            var model = InicializarSubDetalleMantenimiento();

            model.TablaSubDetalle.Accion = Accion.Nuevo;
            model.AccionModel = Accion.Editar;
            
            if (id != null)
            {
                DetalleValor = id;
            }
            return View("EditarDetalle", model);
        }

        /// <summary>
        /// guardar subdetalle 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult SubDetalle(MantenimientoGeneralViewModel model)
        {
            DetalleGeneral detalleGeneral = model.TablaSubDetalle;
            detalleGeneral.Referencia = DetalleValor;

            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var modelDetalle = InicializarDetalleMantenimiento();
                    modelDetalle.TablaDetalleGeneral = detalleGeneral;
                    return View("EditarDetalle", modelDetalle);
                }
                if (detalleGeneral.Accion == Accion.Nuevo)
                {
                    detalleGeneral.EstadoActivo = IndicadorActivo.Activo;
                    detalleGeneral.FechaCreacion = FechaCreacion;
                    string usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                    detalleGeneral.UsuarioCreacion = usuario.Length <= 15? usuario : usuario.Substring(0, 15);
                    detalleGeneral.General = new General();
                    detalleGeneral.General.IdeGeneral = IdeGeneral;
                    if (existeValor(detalleGeneral))
                    {
                        objJsonMessage.Mensaje = "Verifique que los valores ingresados no existan";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        int result = _detalleGeneralRepository.insertarDetalle(detalleGeneral);
                        if (result == 1)
                        {
                            objJsonMessage.Resultado = true;
                            return Json(objJsonMessage);
                        }
                        else
                        {
                            objJsonMessage.Mensaje = "Error: Intente nuevamente";
                            objJsonMessage.Resultado = false;
                            return Json(objJsonMessage);
                        }

                    }
                }
                else
                {
                    if (detalleGeneral.Accion == Accion.Editar)
                    {
                        DetalleGeneral detalleEditar = _detalleGeneralRepository.GetSingle(x => x.IdeGeneral == IdeGeneral && x.Valor == detalleGeneral.Valor);

                        detalleEditar.Valor = detalleGeneral.Valor;
                        detalleEditar.Descripcion = detalleGeneral.Descripcion;
                        string usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                        detalleEditar.UsuarioModificacion = usuario.Length <= 15 ? usuario : usuario.Substring(0, 15);

                        detalleEditar.FechaModificacion = FechaModificacion;
                        _detalleGeneralRepository.Update(detalleEditar);

                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "No se pudo modificar el registro";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }


            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }


        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult ActivarDesactivar(string valor, string descripcion)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if ((valor != null)||(valor != ""))
                {
                    DetalleGeneral detalleEditar = _detalleGeneralRepository.GetSingle(x => x.IdeGeneral == IdeGeneral && x.Valor == valor);
                    string usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                    detalleEditar.UsuarioModificacion = usuario.Length <= 15 ? usuario : usuario.Substring(0, 15);
                    
                    detalleEditar.FechaModificacion = FechaModificacion;
                    if (detalleEditar.EstadoActivo == IndicadorActivo.Activo)
                    {
                        detalleEditar.EstadoActivo = IndicadorActivo.Inactivo;
                    }
                    else
                    {
                        detalleEditar.EstadoActivo = IndicadorActivo.Activo;
                    }
                    _detalleGeneralRepository.Update(detalleEditar);
                    objJsonMessage.Mensaje = "El registro se Activo/Desactivo exitosamente";
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    objJsonMessage.Mensaje = "Error: intente de nuevo";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }

            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
 
        }

        /// <summary>
        /// Determina si el Detalle tiene sub detalle
        /// </summary>
        /// <returns></returns>
        public string determinarIndSubDetalle()
        {
            int contador = _detalleGeneralRepository.CountByExpress(x => x.IdeGeneral == IdeGeneral && x.Referencia != null);

            if (contador == 0)
            {
                return Indicador.No;
            }
            else
            {
                return Indicador.Si;
            }
        }

        /// <summary>
        /// Determina si los valores que se quiere guardar no existan
        /// </summary>
        /// <param name="detalle"></param>
        /// <returns></returns>
        public bool existeValor(DetalleGeneral detalle)
        {
            int contador = _detalleGeneralRepository.CountByExpress(x => x.IdeGeneral == IdeGeneral && x.Valor == detalle.Valor);
            int contDescripcion = _detalleGeneralRepository.CountByExpress(x => x.IdeGeneral == IdeGeneral && x.Descripcion == detalle.Descripcion);

            if ((contador > 0) || (contDescripcion > 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [ValidarSesion]
        public ActionResult ConsultarDetalle(string id)
        {
            var model = InicializarDetalleMantenimiento();
            model.IndSubDetalle = Indicador.No;
            model.TablaDetalleGeneral.Accion = Accion.Nuevo;
            model.AccionModel = Accion.Consultar;

            int idGeneral = Convert.ToInt32(id);
            if (idGeneral != 0)
            {
                IdeGeneral = idGeneral;
                model.IndSubDetalle = determinarIndSubDetalle();

            }
            return View("Edit", model);
        }


        [ValidarSesion]
        public ActionResult ConsultarSubDetalle(string id)
        {
            var model = InicializarSubDetalleMantenimiento();

            model.TablaSubDetalle.Accion = Accion.Nuevo;
            model.AccionModel = Accion.Editar;

            if (id != null)
            {
                DetalleValor = id;
            }
            return View("EditarDetalle", model);
        }
    }
}
        
        
