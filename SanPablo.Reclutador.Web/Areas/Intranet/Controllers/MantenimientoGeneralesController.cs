

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

            mantenimientoViewModel.tipoTablas = new List<General>(_generalRepository.All());

            return mantenimientoViewModel;
        }

        /// <summary>
        /// Crea un nuevo Usuario
        /// </summary>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "TablaDetalleGeneral")]DetalleGeneral detalleGeneral)
        {
            
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var modelDetalle = InicializarDetalleMantenimiento();
                    modelDetalle.TablaDetalleGeneral = detalleGeneral;
                    return View("Edit", modelDetalle);
                }
                if (detalleGeneral.Accion == Accion.Nuevo)
                {
                    detalleGeneral.EstadoActivo = IndicadorActivo.Activo;
                    detalleGeneral.FechaCreacion = FechaCreacion;
                    detalleGeneral.UsuarioCreacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                    _detalleGeneralRepository.Add(detalleGeneral);

                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    if (detalleGeneral.Accion == Accion.Editar)
                    {
                        DetalleGeneral detalleEditar = _detalleGeneralRepository.GetSingle(x => x.IdeGeneral == IdeGeneral && x.Valor == detalleGeneral.Valor);
                    
                        detalleEditar.Valor = detalleGeneral.Valor;
                        detalleEditar.Descripcion = detalleGeneral.Descripcion;
                        detalleEditar.UsuarioModificacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
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


        /// <summary>
        /// Edicion del usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        //public ActionResult Edicion(UsuarioViewModel model)
        //{
        //    Usuario objUsuario;
        //    JsonMessage jsonMessage = new JsonMessage();

        //    if (model.Usuario.IdUsuario!=null && model.Usuario.IdUsuario > 0)
        //    {

        //        objUsuario = new Usuario();

        //        objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(model.Usuario.IdUsuario));
        //        objUsuario.DscApeMaterno = model.Usuario.DscApePaterno;
        //        objUsuario.DscApePaterno = model.Usuario.DscApePaterno;
        //        objUsuario.DscNombres = model.Usuario.DscNombres;
        //        objUsuario.CodUsuario = model.Usuario.CodUsuario;
        //        objUsuario.CodContrasena = model.Usuario.CodContrasena;
        //        objUsuario.Email = model.Usuario.Email;
        //        objUsuario.Telefono = model.Usuario.Telefono;
        //        objUsuario.UsrModificacion = UsuarioActual.NombreUsuario;
        //        objUsuario.FechaModificacion = FechaModificacion;

        //        _usuarioRepository.Update(objUsuario);
        //        jsonMessage.IdDato = objUsuario.IdUsuario;
        //        jsonMessage.Mensaje = "Se actualizo el usuario";
        //        jsonMessage.Resultado = true;


        //    }
        //    else
        //    {
        //        objUsuario = new Usuario();

        //        objUsuario = model.Usuario;
        //        objUsuario.FecCreacion = FechaCreacion;
        //        objUsuario.UsrCreacion = UsuarioActual.NombreUsuario;
        //        objUsuario.FlgEstado = IndicadorActivo.Activo;
        //        objUsuario.TipUsuario = TipUsuario.Instranet;

        //        _usuarioRepository.Add(model.Usuario);
        //        jsonMessage.Mensaje = "Se registro el usuario";
        //        jsonMessage.IdDato = objUsuario.IdUsuario;
        //        jsonMessage.Resultado = true;

        //    }

        //    return Json(jsonMessage);
        //}


        /// <summary>
        /// Editar obtiene los campos de los valores para editar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[ValidarSesion]
        //public ActionResult Edit(string id)
        //{

        //    UsuarioViewModel model = new UsuarioViewModel();
        //    model.Usuario = new Usuario();

        //    var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(id));

        //    model.Usuario = objUsuario;

        //    model.Accion = Accion.Editar;

        //    return View("Edit", model);
        //}
        /// <summary>
        /// Inicializa la Consulta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // [ValidarSesion]
        //public ActionResult Consulta(string id)
        //{
        //    UsuarioViewModel model = new UsuarioViewModel();
        //    model.Usuario = new Usuario();

        //    var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(id));

        //    model.Usuario = objUsuario;

        //    model.Accion = Accion.Consultar;

        //    return View("Edit", model);
        //}







        //[HttpPost]
        //public ActionResult ListaUsuarioRolSede(GridTable grid)
        //{

        //    UsuarioRolSede rs = new UsuarioRolSede();
        //    try
        //    {
        //        DetachedCriteria where = null;

        //        if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)))
        //        {
        //            where = DetachedCriteria.For<UsuarioRolSede>();

        //            if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
        //            {
        //                int dato = Convert.ToInt32(grid.rules[0].data);
        //                where.Add(Expression.Eq("IdUsuario", dato));
        //            }

        //        }

        //        var generic = Listar(_usuarioRolSedeRepository,
        //                             grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
        //        var i = grid.page * grid.rows;

        //        generic.Value.rows = generic.List.Select(item => new Row
        //        {
        //            id = item.IdUsuarolSede.ToString(),
        //            cell = new string[]
        //                    {
                               
        //                        item.IdUsuarolSede==null?"":item.IdUsuarolSede.ToString(),
        //                        item.IdSede==null?"":item.IdSede.ToString(),
        //                        item.SedeDes==null?"No se requiere sede":item.SedeDes,
        //                        item.IdUsuario==null?"":item.IdUsuario.ToString(),
        //                        item.IdRol==null?"":item.IdRol.ToString(),
        //                        item.RolDes==null?"":item.RolDes
                    
        //                    }
        //        }).ToArray();

        //        return Json(generic.Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
        //        return MensajeError();
        //    }
        //}

        /// <summary>
        ///Inicializa el PopupSedeRol
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idSel"></param>
        /// <returns></returns>

        //public ViewResult PopupSedeRol(int id, int idSel)
        //{
        //    UsuarioRolSedeViewModel model = new UsuarioRolSedeViewModel();
        //    model.UsuarioRolSede = new UsuarioRolSede();

        //    model = InicializarPopupSedeRol();

        //    model.IdUsuario = id;
        //    model.IdRolUsuario = idSel;



        //    if (idSel>0)
        //    {

        //        var objUsuario = _usuarioRolSedeRepository.GetSingle(x => x.IdUsuarolSede == id);

        //        if (objUsuario!=null)
        //        {
        //            model.UsuarioRolSede.IdSede = objUsuario.IdSede;
        //            model.UsuarioRolSede.IdRol = objUsuario.IdRol;
        //        }

        //    }

        //    return View("PopupSedeRol", model);


        //}



        /// <summary>
        /// Iniciliza los parametros del popup
        /// </summary>
        /// <returns></returns>
        //private UsuarioRolSedeViewModel InicializarPopupSedeRol()
        //{
        //    var objModel = new UsuarioRolSedeViewModel();
        //    objModel.UsuarioRolSede = new UsuarioRolSede();


        //    objModel.TipRol = new List<Rol>(_rolRepository.GetByTipRol());
        //    objModel.TipRol.Insert(0, new Rol { IdRol = 0, CodRol = "Seleccionar" });


        //    objModel.TipSede = new List<Sede>(_sedeRepository.GetByTipSede());
        //    objModel.TipSede.Insert(0, new Sede { CodigoSede = "0", DescripcionSede = "Seleccionar" });
        //    objModel.UsuarioRolSede = new UsuarioRolSede();
        //    return objModel;
        //}







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
                where = DetachedCriteria.For<UsuarioVista>();

                //ProjectionList lista = Projections.ProjectionList();
                //lista.Add(Projections.Property("FLGESTADO"), "FLGESTADO");
                //lista.Add(Projections.Property("IDUSUARIO"), "IDUSUARIO");
                //lista.Add(Projections.Property("CODUSUARIO"), "CODUSUARIO");
                //lista.Add(Projections.Property("DSCNOMBRES"), "DSCNOMBRES");
                //lista.Add(Projections.Property("DSCAPEPATERNO"), "DSCAPEPATERNO");
                //lista.Add(Projections.Property("DSCAPEMATERNO"), "DSCAPEMATERNO");
                //lista.Add(Projections.Property("DESROL"), "DESROL");
                //lista.Add(Projections.Property("DESSEDE"), "DESSEDE");
                //lista.Add(Projections.Property("TIPUSUARIO"), "TIPUSUARIO");

                //where.SetProjection(Projections.Distinct(lista));
                //where.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<UsuarioVista>());


                //if (
                //    (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                //    (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data)) ||
                //    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0") ||
                //    (!"".Equals(grid.rules[4].data) && grid.rules[4].data != null && grid.rules[4].data != "0") ||
                //    (!"".Equals(grid.rules[5].data) && grid.rules[5].data != null && grid.rules[5].data != "0") ||
                //    (!"".Equals(grid.rules[6].data) && grid.rules[6].data != null && grid.rules[6].data != "0")
                //   )
                //{

                //    if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                //    {

                //        where.Add(Expression.Eq("IDROL", Convert.ToInt32(grid.rules[1].data)));
                //    }
                //    if (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data))
                //    {
                //        where.Add(Expression.Eq("IDESEDE", Convert.ToInt32(grid.rules[2].data)));
                //    }
                //    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                //    {
                //        where.Add(Expression.Like("DSCNOMBRES", '%' + grid.rules[3].data + '%'));
                //    }
                //    if (!"".Equals(grid.rules[4].data) && grid.rules[4].data != null && grid.rules[4].data != "0")
                //    {
                //        where.Add(Expression.Like("CODUSUARIO", '%' + grid.rules[4].data + '%'));

                //    } if (!"".Equals(grid.rules[5].data) && grid.rules[5].data != null && grid.rules[5].data != "0")
                //    {
                //        where.Add(Expression.Like("DSCAPEPATERNO", '%' + grid.rules[5].data + '%'));
                //    }
                //    if (!"".Equals(grid.rules[6].data) && grid.rules[6].data != null && grid.rules[6].data != "0")
                //    {
                //        where.Add(Expression.Like("DSCAPEMATERNO", '%' + grid.rules[6].data + '%'));
                //    }
                //}

               // where.Add(Expression.Eq("TIPUSUARIO", TipUsuario.Instranet));

                var generic = Listar(_generalRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, null);
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
            int idGeneral = Convert.ToInt32(id);
            if (idGeneral != 0)
            {
                IdeGeneral = idGeneral;
            }
            return View("Edit", model);
        }

        public MantenimientoGeneralViewModel InicializarDetalleMantenimiento()
        {
            MantenimientoGeneralViewModel model = new MantenimientoGeneralViewModel();
            model.TablaDetalleGeneral = new DetalleGeneral();

            return model;
        }

        [HttpPost]
        public ActionResult ListaDetalle(GridTable grid)
        {
            try
            {
                DetachedCriteria where = null;
                where = DetachedCriteria.For<DetalleGeneral>();

                //ProjectionList lista = Projections.ProjectionList();
                //lista.Add(Projections.Property("FLGESTADO"), "FLGESTADO");
                //lista.Add(Projections.Property("IDUSUARIO"), "IDUSUARIO");
                //lista.Add(Projections.Property("CODUSUARIO"), "CODUSUARIO");
                //lista.Add(Projections.Property("DSCNOMBRES"), "DSCNOMBRES");
                //lista.Add(Projections.Property("DSCAPEPATERNO"), "DSCAPEPATERNO");
                //lista.Add(Projections.Property("DSCAPEMATERNO"), "DSCAPEMATERNO");
                //lista.Add(Projections.Property("DESROL"), "DESROL");
                //lista.Add(Projections.Property("DESSEDE"), "DESSEDE");
                //lista.Add(Projections.Property("TIPUSUARIO"), "TIPUSUARIO");

                //where.SetProjection(Projections.Distinct(lista));
                //where.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<UsuarioVista>());


                //if (
                //    (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                //    (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data)) ||
                //    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0") ||
                //    (!"".Equals(grid.rules[4].data) && grid.rules[4].data != null && grid.rules[4].data != "0") ||
                //    (!"".Equals(grid.rules[5].data) && grid.rules[5].data != null && grid.rules[5].data != "0") ||
                //    (!"".Equals(grid.rules[6].data) && grid.rules[6].data != null && grid.rules[6].data != "0")
                //   )
                //{

                //    if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                //    {

                //        where.Add(Expression.Eq("IDROL", Convert.ToInt32(grid.rules[1].data)));
                //    }
                //    if (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data))
                //    {
                //        where.Add(Expression.Eq("IDESEDE", Convert.ToInt32(grid.rules[2].data)));
                //    }
                //    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                //    {
                //        where.Add(Expression.Like("DSCNOMBRES", '%' + grid.rules[3].data + '%'));
                //    }
                //    if (!"".Equals(grid.rules[4].data) && grid.rules[4].data != null && grid.rules[4].data != "0")
                //    {
                //        where.Add(Expression.Like("CODUSUARIO", '%' + grid.rules[4].data + '%'));

                //    } if (!"".Equals(grid.rules[5].data) && grid.rules[5].data != null && grid.rules[5].data != "0")
                //    {
                //        where.Add(Expression.Like("DSCAPEPATERNO", '%' + grid.rules[5].data + '%'));
                //    }
                //    if (!"".Equals(grid.rules[6].data) && grid.rules[6].data != null && grid.rules[6].data != "0")
                //    {
                //        where.Add(Expression.Like("DSCAPEMATERNO", '%' + grid.rules[6].data + '%'));
                //    }
                //}

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
                            item.Descripcion==null?"":item.Descripcion.ToString()
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
        

        /// <summary>
        /// Activar y desactivar usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codEstado"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        //public ActionResult ActivarDesactivar(string id, string codEstado)
        //{
        //    JsonMessage objJsonMessage = new JsonMessage();
        //    Usuario objUsuario = new Usuario();
        //    try
        //    {
        //        objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(id));

        //        if (IndicadorActivo.Activo.Equals(codEstado))
        //        {
        //            objUsuario.FlgEstado = IndicadorActivo.Inactivo;
        //            objJsonMessage.Mensaje = "Se desactivado el usuario";
        //        }
        //        else
        //        {
        //            objUsuario.FlgEstado = IndicadorActivo.Activo;
        //            objJsonMessage.Mensaje = "Se activo el usuario";
        //        }

        //        objJsonMessage.Resultado = true;


        //    }
        //    catch (Exception)
        //    {

        //        objJsonMessage.Resultado = false;
        //        objJsonMessage.Mensaje = "Error en actualizar el estado";
        //    }

        //    _usuarioRepository.Update(objUsuario);

        //    return Json(objJsonMessage);
        //}

    }
}
        
        
