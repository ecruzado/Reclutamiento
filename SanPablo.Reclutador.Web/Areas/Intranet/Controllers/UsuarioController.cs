

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
    public class UsuarioController : BaseController
    {
       
        private IRolRepository _rolRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IRolOpcionRepository _rolOpcionRepository;
        private IUsuarioRepository _usuarioRepository;
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;
        private ISedeRepository _sedeRepository;
        private IUsuarioVistaRepository _usuarioVistaRepository;
        private ITipoRequerimiento _tipoRequerimiento;

        public UsuarioController(IRolRepository rolRepository, IDetalleGeneralRepository detalleGeneralRepository,
                             IRolOpcionRepository rolOpcionRepository, IUsuarioRepository usuarioRepository,
                             IUsuarioRolSedeRepository usuarioRolSedeRepository,
                             ISedeRepository sedeRepository,
                             IUsuarioVistaRepository usuarioVistaRepository,
                             ITipoRequerimiento tipoRequerimiento
            )
        {
            _rolRepository = rolRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _rolOpcionRepository = rolOpcionRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _sedeRepository = sedeRepository;
            _usuarioVistaRepository = usuarioVistaRepository;
            _tipoRequerimiento = tipoRequerimiento;
        }

        /// <summary>
        /// inicializa la pantalla inicial de usuarios
        /// </summary>
        /// <returns></returns>
        
        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult Index()
        {
            UsuarioRolSedeViewModel objModel = new UsuarioRolSedeViewModel();
           

            objModel = InicializarPopupSedeRol();
            objModel.Usuario = new Usuario();
            objModel.UsuarioRolSede = new UsuarioRolSede();
            
            return View("Index",objModel);
        }


       
        /// <summary>
        /// Crea un nuevo Usuario
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Nuevo()
        {
            UsuarioViewModel usuModel = new UsuarioViewModel();

            usuModel.Usuario = new Usuario();
            usuModel.Accion = Accion.Nuevo;

            return View("Edit", usuModel);
        }
        
        /// <summary>
        /// Valida que el codigo de usuario sea unico
        /// </summary>
        /// <param name="codUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult valCodUsuario(string id, string accion)
        {
            UsuarioViewModel usuModel = new UsuarioViewModel();
            JsonMessage jsonMessage = new JsonMessage();
            Usuario objUsuario = new Usuario();

            if (Accion.Nuevo.Equals(accion))
            {
                 objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id);
                 
                 if (objUsuario != null)
                 {
                     jsonMessage.Resultado = false;
                     jsonMessage.Mensaje = "El código de usuario ya se encuentra registrado por favor registrar uno diferente";
                 }
                 else
                 {
                     jsonMessage.Resultado = true;

                 }
            }
            else
            {
                jsonMessage.Resultado = true;
            }
            
            return Json(jsonMessage);
        }


        /// <summary>
        /// Edicion del usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult Edicion(UsuarioViewModel model)
        {
            Usuario objUsuario;
            JsonMessage jsonMessage = new JsonMessage();

            if (model.Usuario.IdUsuario!=null && model.Usuario.IdUsuario > 0)
            {

                objUsuario = new Usuario();

                objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(model.Usuario.IdUsuario));
                objUsuario.DscApeMaterno = model.Usuario.DscApePaterno;
                objUsuario.DscApePaterno = model.Usuario.DscApePaterno;
                objUsuario.DscNombres = model.Usuario.DscNombres;
                objUsuario.CodUsuario = model.Usuario.CodUsuario;
                objUsuario.CodContrasena = model.Usuario.CodContrasena;
                objUsuario.Email = model.Usuario.Email;
                objUsuario.Telefono = model.Usuario.Telefono;
                objUsuario.UsrModificacion = UsuarioActual.NombreUsuario;
                objUsuario.FechaModificacion = FechaModificacion;

                _usuarioRepository.Update(objUsuario);
                jsonMessage.IdDato = objUsuario.IdUsuario;
                jsonMessage.Mensaje = "Se actualizo el usuario";
                jsonMessage.Resultado = true;


            }
            else
            {
                objUsuario = new Usuario();

                objUsuario = model.Usuario;
                objUsuario.FecCreacion = FechaCreacion;
                objUsuario.UsrCreacion = UsuarioActual.NombreUsuario;
                objUsuario.FlgEstado = "A";
                _usuarioRepository.Add(model.Usuario);
                jsonMessage.Mensaje = "Se registro el usuario";
                jsonMessage.IdDato = objUsuario.IdUsuario;
                jsonMessage.Resultado = true;

            }

            return Json(jsonMessage);
        }


        /// <summary>
        /// Editar obtiene los campos de los valores para editar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Edit(string id)
        {

            UsuarioViewModel model = new UsuarioViewModel();
            model.Usuario = new Usuario();
            
            var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(id));

            model.Usuario = objUsuario;

            model.Accion = Accion.Editar;

            return View("Edit", model);
        }
        /// <summary>
        /// Inicializa la Consulta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         [ValidarSesion]
        public ActionResult Consulta(string id)
        {
            UsuarioViewModel model = new UsuarioViewModel();
            model.Usuario = new Usuario();

            var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(id));

            model.Usuario = objUsuario;

            model.Accion = Accion.Consultar;

            return View("Edit", model);
        }
       


        /// <summary>
        /// obtiene le indicador de la sede
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetIndSede(string id)
        {
            JsonMessage objJson = new JsonMessage();
            int codRol  = 0;
            string inSede;
            Rol objRol;
            if (id!=null)
            {
                codRol = Convert.ToInt32(id);
                objRol = new Rol();   
                objRol = _rolRepository.GetSingle(x => x.IdRol == codRol);
                if (objRol!=null)
                {
                   inSede= objRol.FlgSede;
                   if ("S".Equals(inSede))
                   {
                       objJson.Resultado = true;
                   }
                   else
                   {
                       objJson.Resultado = false;
                   }
                }

               

            }

            
            return Json(objJson);
        }
        
       


        [HttpPost]
        public ActionResult ListaUsuarioRolSede(GridTable grid)
        {

            UsuarioRolSede rs = new UsuarioRolSede();
            try
            {
                DetachedCriteria where = null;

                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)))
                {
                    where = DetachedCriteria.For<UsuarioRolSede>();

                    if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
                    {
                        int dato = Convert.ToInt32(grid.rules[0].data);
                        where.Add(Expression.Eq("IdUsuario", dato));
                    }

                }

                var generic = Listar(_usuarioRolSedeRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdUsuarolSede.ToString(),
                    cell = new string[]
                            {
                               
                                item.IdUsuarolSede==null?"":item.IdUsuarolSede.ToString(),
                                item.IdSede==null?"":item.IdSede.ToString(),
                                item.SedeDes==null?"No se requiere sede":item.SedeDes,
                                item.IdUsuario==null?"":item.IdUsuario.ToString(),
                                item.IdRol==null?"":item.IdRol.ToString(),
                                item.RolDes==null?"":item.RolDes
                    
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
        ///Inicializa el PopupSedeRol
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idSel"></param>
        /// <returns></returns>
        
        public ViewResult PopupSedeRol(int id, int idSel)
        {
            UsuarioRolSedeViewModel model = new UsuarioRolSedeViewModel();
            model.UsuarioRolSede = new UsuarioRolSede();
            
            model = InicializarPopupSedeRol();

            model.IdUsuario = id;
            model.IdRolUsuario = idSel;

            

            if (idSel>0)
            {
               
                var objUsuario = _usuarioRolSedeRepository.GetSingle(x => x.IdUsuarolSede == id);

                if (objUsuario!=null)
                {
                    model.UsuarioRolSede.IdSede = objUsuario.IdSede;
                    model.UsuarioRolSede.IdRol = objUsuario.IdRol;
                }

            }

            return View(model);
            

        }


        /// <summary>
        /// Iniciliza popup tipo de requerimiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult inicioPopupTipReq(int id)
        {
            UsuarioRolSedeViewModel model = new UsuarioRolSedeViewModel();
            model.UsuarioRolSede = new UsuarioRolSede();

            

            return View("PopupTipoReq",model);

        }


        /// <summary>
        /// Iniciliza los parametros del popup
        /// </summary>
        /// <returns></returns>
        private UsuarioRolSedeViewModel InicializarPopupSedeRol()
        {
            var objModel = new UsuarioRolSedeViewModel();
            objModel.UsuarioRolSede = new UsuarioRolSede();


            objModel.TipRol = new List<Rol>(_rolRepository.GetByTipRol());
            objModel.TipRol.Insert(0, new Rol { IdRol = 0,  CodRol= "Seleccionar" });
            

            objModel.TipSede = new List<Sede>(_sedeRepository.GetByTipSede());
            objModel.TipSede.Insert(0, new Sede { CodigoSede = "0", DescripcionSede = "Seleccionar" });
            objModel.UsuarioRolSede = new UsuarioRolSede();
            return objModel;
        }


        
        /// <summary>
        /// obtiene la los datos del popup
        /// </summary>
        /// <param name="selc"></param>
        /// <param name="codExamen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetPopupSedeRol(UsuarioRolSedeViewModel popupModel)
        {
            JsonMessage objJson = new JsonMessage();
            UsuarioRolSede objUsuarioRolSede;

            int selc = popupModel.IdUsuario;
            

            if (popupModel != null)
            {

                popupModel.UsuarioRolSede.IdUsuario = popupModel.IdUsuario;
                objUsuarioRolSede = new UsuarioRolSede();

                var objRol = _rolRepository.GetSingle(x => x.IdRol == popupModel.UsuarioRolSede.IdRol);

                    if ("S".Equals(objRol.FlgSede))
                    {
                        objUsuarioRolSede.IdSede = popupModel.UsuarioRolSede.IdSede;
                    }
                    else
                    {
                        objUsuarioRolSede.IdSede = 0;
                    }

                    objUsuarioRolSede.IdRol = popupModel.UsuarioRolSede.IdRol;
                    objUsuarioRolSede.IdUsuario = popupModel.UsuarioRolSede.IdUsuario;
                    objUsuarioRolSede.FechaCreacion = FechaCreacion;
                    objUsuarioRolSede.UsuarioCreacion = UsuarioActual.UsuarioCreacion;
                    
                    _usuarioRolSedeRepository.Add(objUsuarioRolSede);
                    objJson.Resultado = true;
                    objJson.Mensaje = "Se grabo el registro";
 
            }

            return Json(objJson); ;
        }


        /// <summary>
        /// Se elemina la relacion rol Sede
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult EliminaRolSede(string id)
        {
            JsonMessage objJson = new JsonMessage();
           
            if (id != null)
            {

                var ObjRolSede =_usuarioRolSedeRepository.GetSingle(x => x.IdUsuarolSede == Convert.ToInt32(id));
                _usuarioRolSedeRepository.Remove(ObjRolSede);
                objJson.Resultado = true;
                objJson.Mensaje = "Se elimino el registro";

            }

            return Json(objJson); ;
        }


        /// <summary>
        /// ListaUsuarios lista de usuarios
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaUsuarios(GridTable grid)
        {
            try
            {
               
                DetachedCriteria where = null;
                where = DetachedCriteria.For<UsuarioVista>();
                
                ProjectionList lista = Projections.ProjectionList();
                lista.Add(Projections.Property("FLGESTADO"), "FLGESTADO");
                lista.Add(Projections.Property("IDUSUARIO"), "IDUSUARIO");
                lista.Add(Projections.Property("CODUSUARIO"), "CODUSUARIO");
                lista.Add(Projections.Property("DSCNOMBRES"), "DSCNOMBRES");
                lista.Add(Projections.Property("DSCAPEPATERNO"), "DSCAPEPATERNO");
                lista.Add(Projections.Property("DSCAPEMATERNO"), "DSCAPEMATERNO");
                lista.Add(Projections.Property("DESROL"), "DESROL");
                lista.Add(Projections.Property("DESSEDE"), "DESSEDE");
               
                where.SetProjection(Projections.Distinct(lista));
                where.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<UsuarioVista>());

                
                if(
                    (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data)) ||
                    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0") ||
                    (!"".Equals(grid.rules[4].data) && grid.rules[4].data != null && grid.rules[4].data != "0") ||
                    (!"".Equals(grid.rules[5].data) && grid.rules[5].data != null && grid.rules[5].data != "0") ||
                    (!"".Equals(grid.rules[6].data) && grid.rules[6].data != null && grid.rules[6].data != "0")
                   )
                {

                    if (!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                    {

                        where.Add(Expression.Eq("IDROL",Convert.ToInt32(grid.rules[1].data)));
                    }
                    if (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data))
                    {
                        where.Add(Expression.Eq("IDESEDE", Convert.ToInt32(grid.rules[2].data)));
                    }
                    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                    {
                        where.Add(Expression.Like("DSCNOMBRES", '%' + grid.rules[3].data + '%'));
                    }
                    if (!"".Equals(grid.rules[4].data) && grid.rules[4].data != null && grid.rules[4].data != "0")
                    {
                        where.Add(Expression.Like("CODUSUARIO", '%' + grid.rules[4].data + '%'));

                    } if (!"".Equals(grid.rules[5].data) && grid.rules[5].data != null && grid.rules[5].data != "0")
                    {
                        where.Add(Expression.Like("DSCAPEPATERNO", '%' + grid.rules[5].data + '%'));
                    }
                    if (!"".Equals(grid.rules[6].data) && grid.rules[6].data != null && grid.rules[6].data != "0")
                    {
                        where.Add(Expression.Like("DSCAPEMATERNO", '%' + grid.rules[6].data + '%'));
                    }
                }

                var generic = Listar(_usuarioVistaRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDUSUARIO.ToString(),
                    cell = new string[]
                            {
                                "1",
                                item.FLGESTADO==null?"":item.FLGESTADO.ToString(),
                                item.IDUSUARIO==null?"":item.IDUSUARIO.ToString(),
                                item.CODUSUARIO==null?"":item.CODUSUARIO,
                                item.DSCNOMBRES==null?"":item.DSCNOMBRES,
                                item.DSCAPEPATERNO==null?"":item.DSCAPEPATERNO,
                                item.DSCAPEMATERNO==null?"":item.DSCAPEMATERNO,
                                item.DESROL==null?"":item.DESROL,
                                item.DESSEDE==null?"":item.DESSEDE
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
        /// Elimina el Usuario definitivamente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult EliminarUsuario(string id)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            Usuario objUsuario = new Usuario();
            try
            {
                objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(id));
                _usuarioRepository.Remove(objUsuario);
                objJsonMessage.Resultado = true;
                objJsonMessage.Mensaje = "Se elimino el registro";

            }
            catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "Error al eliminar el registro";
            }



            return Json(objJsonMessage);
        }


        /// <summary>
        /// Activar y desactivar usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codEstado"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult ActivarDesactivar(string id, string codEstado)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            Usuario objUsuario = new Usuario();
            try
            {
                objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(id));

                if (IndicadorActivo.Activo.Equals(codEstado))
                {
                    objUsuario.FlgEstado = IndicadorActivo.Inactivo;
                    objJsonMessage.Mensaje = "Se desactivado el usuario";
                }
                else
                {
                    objUsuario.FlgEstado = IndicadorActivo.Activo;
                    objJsonMessage.Mensaje = "Se activo el usuario";
                }

                objJsonMessage.Resultado = true;


            }
            catch (Exception)
            {

                objJsonMessage.Resultado = false;
                objJsonMessage.Mensaje = "Error en actualizar el estado";
            }

            _usuarioRepository.Update(objUsuario);

            return Json(objJsonMessage);
        }


        
        /// <summary>
        /// Inicializa el popup de cambio de pass
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>s
        [ValidarSesion]
        public ViewResult InicializaPopup()
        {
            UsuarioRolSedeViewModel objModel = new UsuarioRolSedeViewModel();
            objModel.Password = new Password();

            return View("PopupPassword", objModel);

         }

        /// <summary>
        /// Lista los tipo de requerimiento
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaPopupTipoReq(GridTable grid)
        {
            try
            {
                
                
                DetachedCriteria where = null;
                where = DetachedCriteria.For<DetalleGeneral>();

                where.Add(Expression.Eq("IDEGENERAL", TipoTabla.TipoMenu));

                var generic = Listar(_detalleGeneralRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.Valor.ToString(),
                    cell = new string[]
                            {
                                
                                item.Valor==null?"":item.Valor,
                                item.Descripcion==null?"":item.Descripcion
                            }
                }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                
                return MensajeError();
            }
        }

        



    }
}
