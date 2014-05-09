

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
        private ISedeNivelRepository _sedeNivelRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IAreaRepository _areaRepository;

        public UsuarioController(IRolRepository rolRepository, IDetalleGeneralRepository detalleGeneralRepository,
                             IRolOpcionRepository rolOpcionRepository, IUsuarioRepository usuarioRepository,
                             IUsuarioRolSedeRepository usuarioRolSedeRepository,
                             ISedeRepository sedeRepository,
                             IUsuarioVistaRepository usuarioVistaRepository,
                             ITipoRequerimiento tipoRequerimiento,
                             ISedeNivelRepository sedeNivelRepository,
                             IDependenciaRepository dependenciaRepository,
                             IDepartamentoRepository departamentoRepository,
                             IAreaRepository areaRepository
            
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
            _sedeNivelRepository = sedeNivelRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            _areaRepository = areaRepository;
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
            
            //Accesos a los botones

            int idRol = (Session[ConstanteSesion.Rol] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Rol]));

            if (Roles.Administrador_Sistema.Equals(idRol))
            {
                objModel.btnActivarDesactivar = Visualicion.SI;
                objModel.btnBuscar = Visualicion.SI;
                objModel.btnLimpiar = Visualicion.SI;

                objModel.btnNuevo = Visualicion.SI;
                objModel.btnConsultar = Visualicion.SI;
                objModel.btnEditar = Visualicion.SI;
                objModel.btnEliminar = Visualicion.SI;

            }
            else if (Roles.Encargado_Seleccion.Equals(idRol))
            {
                objModel.btnActivarDesactivar = Visualicion.SI;
                objModel.btnBuscar = Visualicion.SI;
                objModel.btnLimpiar = Visualicion.SI;

                objModel.btnNuevo = Visualicion.NO;
                objModel.btnConsultar = Visualicion.SI;
                objModel.btnEditar = Visualicion.NO;
                objModel.btnEliminar = Visualicion.NO;

            }
            else if (Roles.Analista_Seleccion.Equals(idRol))
            {
                objModel.btnActivarDesactivar = Visualicion.SI;
                objModel.btnBuscar = Visualicion.SI;
                objModel.btnLimpiar = Visualicion.SI;

                objModel.btnNuevo = Visualicion.NO;
                objModel.btnConsultar = Visualicion.SI;
                objModel.btnEditar = Visualicion.NO;
                objModel.btnEliminar = Visualicion.NO;
            }
            else
            {
                objModel.btnActivarDesactivar = Visualicion.NO;
                objModel.btnBuscar = Visualicion.NO;
                objModel.btnLimpiar = Visualicion.NO;

                objModel.btnNuevo = Visualicion.NO;
                objModel.btnConsultar = Visualicion.NO;
                objModel.btnEditar = Visualicion.NO;
                objModel.btnEliminar = Visualicion.NO;
            }

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
                 objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id && x.TipUsuario == TipUsuario.Instranet);
                 
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

                objUsuario.UsrModificacion = UsuarioActual.NombreUsuario;
                objUsuario.FecModifcacion = FechaCreacion;

                objUsuario.UsrCreacion = UsuarioActual.NombreUsuario;

                objUsuario.FlgEstado = IndicadorActivo.Activo;
                objUsuario.TipUsuario = TipUsuario.Instranet;

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

            return View("PopupSedeRol", model);
            

        }

        /// <summary>
        /// Inicializa el maestro de sedes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idSel"></param>
        /// <returns></returns>
        public ViewResult PopupSede(int id, int idSel)
        {
            UsuarioRolSedeViewModel model = new UsuarioRolSedeViewModel();
            model.UsuarioRolSede = new UsuarioRolSede();

            model.IdUsuario = id;


            return View("PopupSede", model);


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

            model.IdUsuario = id;
           


            return View("PopupTipoReq",model);

        }


        /// <summary>
        /// Valida si el tipo de rol es uno de recursos humanos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidaTipoRol(int id)
        {
            JsonMessage objJson = new JsonMessage();


            string IndGrilla = Visualicion.NO;

            var ListaRoles =  _usuarioRolSedeRepository.GetListaRol(id);

            if (ListaRoles!=null)
            {
                foreach (Rol item in ListaRoles)
                {
                    if (Roles.Encargado_Seleccion.Equals(item.IdRol) || Roles.Analista_Seleccion.Equals(item.IdRol))
                    {
                        IndGrilla = Visualicion.SI;
                    }
                }
            }

            if (Visualicion.SI.Equals(IndGrilla))
	        {
		       objJson.Resultado = true;
	        }else
	        {
               objJson.Resultado = false;
               objJson.Mensaje = "Debe tener un rol encargado de selección o analista de selección para poder asignar un tipo de requerimiento";
	        }

            return Json(objJson);

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
        /// obtiene la los datos del popup Sede x Rol
        /// </summary>
        /// <param name="selc"></param>
        /// <param name="codExamen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetPopupSedeRol(List<int> selc, int idRol, int idUsu, string indSede)
        {


            JsonMessage objJson = new JsonMessage();
            UsuarioRolSede objUsuarioRolSede;
            int idSede = 0;
            int indGrabo = 0;
            objUsuarioRolSede = new UsuarioRolSede();

            if (idRol!=null && idRol>0)
            {
                objUsuarioRolSede = new UsuarioRolSede();


                if ("S".Equals(indSede))
                {
                    for (int i = 0; i < selc.Count; i++)
                    {
                        idSede = selc[i];

                        indGrabo = insertaRolSede(idUsu, idRol, idSede);
                    }

                }
                else
                {
                    indGrabo = insertaRolSede(idUsu, idRol, 0);
                }

            }

            if (indGrabo>0)
            {
                objJson.Mensaje = "Se registraron los datos correctamente";
                objJson.Resultado = true;
            }
            else
            {
                objJson.Mensaje = "Error, Consulte con el area de sistemas";
                objJson.Resultado = false;
            }

            return Json(objJson); ;
        }


        /// <summary>
        /// Inserta el rol y la Sede del del usuario
        /// </summary>
        /// <param name="idUsu"></param>
        /// <param name="idRol"></param>
        /// <param name="idSede"></param>
        /// <returns></returns>
        public int insertaRolSede (int idUsu,int idRol, int idSede){

            int indGrabo = 0;
            SedeNivel objSedeNivel;
            try
            {

                UsuarioRolSede objUsuarioRolSede;

                var obj =  _usuarioRolSedeRepository.GetSingle(x => x.IdRol == idRol
                                                && x.IdUsuario == idUsu
                                                && x.IdSede == idSede );

                if (obj==null)
                {
                    objUsuarioRolSede = new UsuarioRolSede();
                    objUsuarioRolSede.IdRol = idRol;
                    objUsuarioRolSede.IdUsuario = idUsu;
                    objUsuarioRolSede.IdSede = idSede;
                    objUsuarioRolSede.FechaModificacion = FechaCreacion;
                    objUsuarioRolSede.UsuarioModificacion = UsuarioActual.UsuarioCreacion;
                    objUsuarioRolSede.FechaCreacion = FechaCreacion;
                    objUsuarioRolSede.UsuarioCreacion = UsuarioActual.UsuarioCreacion;
                    _usuarioRolSedeRepository.Add(objUsuarioRolSede);
                }

               
                
                indGrabo = 1;
            }
            catch (Exception)
            {

                indGrabo = 0;
            }

            return indGrabo;

        }


        /// <summary>
        /// Obtiene las Sedes del maestro Sede y las graba
        /// </summary>
        /// <param name="selc"></param>
        /// <param name="idRol"></param>
        /// <param name="idUsu"></param>
        /// <param name="indSede"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetPopupSede(List<int> selc,int idUsu)
        {


            JsonMessage objJson = new JsonMessage();
            SedeNivel objSedeNivel;
            int idSede = 0;
            int indGrabo = 0;
           

            if (idUsu != null && idUsu > 0)
            {
                if (selc.Count>0)
                {

                    for (int i = 0; i < selc.Count; i++)
                    {
                        idSede = selc[i];

                        if (idSede!=0)
                        {
                            var listaSedes = _sedeNivelRepository.GetBy(x => x.IDESEDE==idSede 
                                                            && x.IDUSUARIO==idUsu);
                            if (listaSedes.Count == 0)
                            {
                                objSedeNivel = new SedeNivel();
                                objSedeNivel.IDUSUARIO = idUsu;
                                objSedeNivel.IDESEDE = idSede;
                                objSedeNivel.FechaCreacion = FechaCreacion;
                                objSedeNivel.UsuarioCreacion = UsuarioActual.NombreUsuario;
                                objSedeNivel.FechaModificacion = FechaModificacion;
                                objSedeNivel.UsuarioModificacion = UsuarioActual.NombreUsuario;
                                objSedeNivel.FLGESTADO = IndicadorActivo.Activo;

                                _sedeNivelRepository.Add(objSedeNivel);
                                indGrabo = 1;     
                            }
                        }
                    } 
                }

            }

            if (indGrabo > 0)
            {
                objJson.Mensaje = "Se registraron los datos correctamente";
                objJson.Resultado = true;
            }
            else
            {
                objJson.Mensaje = "Error, Consulte con el area de sistemas";
                objJson.Resultado = false;
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
        /// Elimina la relacion de rol y sede
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult EliminaRolSedeNivel(string id)
        {
            JsonMessage objJson = new JsonMessage();
           
            if (id != null)
            {
                var ObjSedeNivel =_sedeNivelRepository.GetSingle(x => x.IDUSUARIONIVEL == Convert.ToInt32(id));

                if (ObjSedeNivel!=null)
                {
                      var listaRolSede =_usuarioRolSedeRepository.GetBy(x => x.IdSede == ObjSedeNivel.IDESEDE &&
                                                                          x.IdUsuario == ObjSedeNivel.IDUSUARIO );
                      if (listaRolSede!=null && listaRolSede.Count>0)
                      {
                          objJson.Resultado = false;
                          objJson.Mensaje = "No se puede eliminar la sede, tiene roles asociados";

                      }
                      else
                      {
                          _sedeNivelRepository.Remove(ObjSedeNivel);
                          objJson.Resultado = true;
                          objJson.Mensaje = "Se elimino el registro";
                      }

                }

            }

            return Json(objJson); ;
        }


        

        /// <summary>
        /// Elimina tipo de requerimiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult EliminarTipReq(string id,string tipReq)
        {
             
            JsonMessage objJson = new JsonMessage();

            if (id != null && tipReq!=null)
            {
                var objUsuTipReq = _tipoRequerimiento.GetSingle(x => x.IDUSUARIO == Convert.ToInt32(id) && x.TIPREQ == tipReq );

                if (objUsuTipReq!=null)
                {
                    _tipoRequerimiento.Remove(objUsuTipReq);
                    objJson.Resultado = true;
                    objJson.Mensaje = "Se elimino el registro";
                }                                

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

                List<UsuarioVista> listaUsuarios;
                UsuarioVista objUsuarioVista;

                objUsuarioVista = new UsuarioVista();
                //DetachedCriteria where = null;
                //where = DetachedCriteria.For<UsuarioVista>();
                
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
                        objUsuarioVista.IDROL = Convert.ToInt32(grid.rules[1].data);
                        //where.Add(Expression.Eq("IDROL",Convert.ToInt32(grid.rules[1].data)));
                    }
                    if (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data))
                    {
                        objUsuarioVista.IDESEDE = Convert.ToInt32(grid.rules[2].data);
                        //where.Add(Expression.Eq("IDESEDE", Convert.ToInt32(grid.rules[2].data)));
                    }
                    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                    {
                        
                        objUsuarioVista.DSCNOMBRES = grid.rules[3].data;
                        //where.Add(Expression.Like("DSCNOMBRES", '%' + grid.rules[3].data + '%'));
                    }
                    if (!"".Equals(grid.rules[4].data) && grid.rules[4].data != null && grid.rules[4].data != "0")
                    {
                        objUsuarioVista.CODUSUARIO = grid.rules[4].data;
                        //where.Add(Expression.Like("CODUSUARIO", '%' + grid.rules[4].data + '%'));

                    } 
                    if (!"".Equals(grid.rules[5].data) && grid.rules[5].data != null && grid.rules[5].data != "0")
                    {
                         objUsuarioVista.DSCAPEPATERNO = grid.rules[5].data;
                        
                        //where.Add(Expression.Like("DSCAPEPATERNO", '%' + grid.rules[5].data + '%'));
                    }
                    if (!"".Equals(grid.rules[6].data) && grid.rules[6].data != null && grid.rules[6].data != "0")
                    {
                        
                        objUsuarioVista.DSCAPEMATERNO = grid.rules[6].data;
                        //where.Add(Expression.Like("DSCAPEMATERNO", '%' + grid.rules[6].data + '%'));
                    }
                }

                //where.Add(Expression.Eq("TIPUSUARIO", TipUsuario.Instranet));
                listaUsuarios = new List<UsuarioVista>();

                listaUsuarios = _usuarioRepository.GetUsuarioVista(objUsuarioVista);

                var generic = GetListar(listaUsuarios,
                                        grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

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
            UsuarioRolSede objUsuarioRolSede = new UsuarioRolSede();
            TipoRequerimiento objTipoReq = new TipoRequerimiento();

            int idUsuario = Convert.ToInt32(id);


            SedeNivel objUsuNivel = new SedeNivel();
            try
            {
                //elimina de la tabla usuario 
                objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == idUsuario);
                if (objUsuario!=null)
                {
                    _usuarioRepository.Remove(objUsuario);    
                }
                

                // elimina la relaicon con las sedes
                objUsuNivel = _sedeNivelRepository.GetSingle(x => x.IDUSUARIO == idUsuario);
                if (objUsuNivel!=null)
                {
                    _sedeNivelRepository.Remove(objUsuNivel);
                }
                

                //elimina la relacion con la sede y el rol
                objUsuarioRolSede = _usuarioRolSedeRepository.GetSingle(x => x.IdUsuario == idUsuario);
                if (objUsuarioRolSede!=null)
                {
                    _usuarioRolSedeRepository.Remove(objUsuarioRolSede);
                }
               

                //elimina la relacion con el tipo de requerimiento
                objTipoReq = _tipoRequerimiento.GetSingle(x => x.IDUSUARIO == idUsuario);
                if (objTipoReq!=null)
                {
                    _tipoRequerimiento.Remove(objTipoReq);
                }
                

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

                int codReg = Convert.ToInt32(TipoTabla.TipoRequerimiento);

                where.Add(Expression.Eq("IdeGeneral", codReg));
                where.Add(Expression.Eq("EstadoActivo", IndicadorActivo.Activo));

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

        /// <summary>
        /// obtiene la lista de requerimientos seleccionados
        /// </summary>
        /// <param name="selc"></param>
        /// <param name="codExamen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetPopupTipoReq(List<string> selc, string codUsuario)
        {
            DateTime Hoy = DateTime.Today;
            TipoRequerimiento objTipoRequerimiento;
            JsonMessage objJson = new JsonMessage();
            int codigo = 0;
            string codReq = null;

            if (codUsuario != null)
            {
                codigo = Convert.ToInt32(codUsuario);
            }
            else
            {
                codigo = 0;
            }

            if (selc != null && selc.Count > 0)
            {
                for (int i = 0; i < selc.Count; i++)
                {
                    codReq = selc[i] == null ? "" : selc[i];
                    
                    var objTipoReq= _tipoRequerimiento.GetBy(x => x.IDUSUARIO == Convert.ToInt32(codUsuario) 
                                                             &&   x.TIPREQ == codReq );

                    if (objTipoReq != null && objTipoReq.Count > 0)
                    {
                        continue;
                    }
                    else
                    {
                        objTipoRequerimiento = new TipoRequerimiento();

                        objTipoRequerimiento.FechaCreacion = FechaCreacion;
                        objTipoRequerimiento.FechaModificacion = FechaModificacion;
                        objTipoRequerimiento.UsuarioCreacion = UsuarioActual.NombreUsuario;
                        objTipoRequerimiento.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        
                        objTipoRequerimiento.IDUSUARIO = Convert.ToInt32(codUsuario);
                        objTipoRequerimiento.TIPREQ = codReq;
                        
                        _tipoRequerimiento.Add(objTipoRequerimiento);
                        
                        objJson.Mensaje = "Se registraron los tipos de requerimiento correctamente";
                        objJson.Resultado = true;

                    }

                }
            }

            return Json(objJson); ;
        }

        /// <summary>
        /// obtiene la lista de tipo de requerimientos
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetListaTipoReq(GridTable grid)
        {

            TipoRequerimiento rs = new TipoRequerimiento();
            try
            {
                DetachedCriteria where = null;

                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)))
                {
                    where = DetachedCriteria.For<TipoRequerimiento>();

                    if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
                    {
                        int dato = Convert.ToInt32(grid.rules[0].data);
                        where.Add(Expression.Eq("IDUSUARIO", dato));
                    }

                }

                var generic = Listar(_tipoRequerimiento,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDUSUREQ.ToString(),
                    cell = new string[]
                            {
                               
                                item.IDUSUREQ==null?"":item.IDUSUREQ.ToString(),
                                item.IDUSUARIO==null?"":item.IDUSUARIO.ToString(),
                                item.TIPREQ==null?"":item.TIPREQ.ToString(),
                                item.DESREQ==null?"":item.DESREQ
                               
                    
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
        /// Lista todas las sedes del maestro sede
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaPopupSedesInicio(GridTable grid)
        {
            try
            {
                DetachedCriteria where = null;
                where = DetachedCriteria.For<Sede>();

               // int codReg = Convert.ToInt32(TipoTabla.TipoRequerimiento);

                where.Add(Expression.Eq("EstadoRegistro", "A"));
                

                var generic = Listar(_sedeRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.CodigoSede.ToString(),
                    cell = new string[]
                            {
                                
                                item.CodigoSede==null?"":item.CodigoSede,
                                item.DescripcionSede==null?"":item.DescripcionSede
                            }
                }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }


        /// <summary>
        /// obtiene solo las sedes registradas para un usuario especifico
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaPopupSedexUsuario(GridTable grid)
        {
            try
            {
                DetachedCriteria where = null;
                //where = DetachedCriteria.For<Sede>();

                // int codReg = Convert.ToInt32(TipoTabla.TipoRequerimiento);

                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)))
                {
                    where = DetachedCriteria.For<SedeNivel>();

                    if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
                    {
                        int dato = Convert.ToInt32(grid.rules[0].data);
                        where.Add(Expression.Eq("IDUSUARIO", dato));
                    }

                }

                where.Add(Expression.Eq("FLGESTADO", "A"));


                var generic = Listar(_sedeNivelRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDESEDE.ToString(),
                    cell = new string[]
                            {
                                
                                item.IDESEDE==null?"":item.IDESEDE.ToString(),
                                item.SEDEDES==null?"":item.SEDEDES
                            }
                }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }


        /// <summary>
        /// obtiene las sedes, dependencia, departamento, area
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSedeNivel(GridTable grid)
        {

            try
            {
                DetachedCriteria where = null;

                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)))
                {
                    where = DetachedCriteria.For<SedeNivel>();

                    if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
                    {
                        int dato = Convert.ToInt32(grid.rules[0].data);
                        where.Add(Expression.Eq("IDUSUARIO", dato));
                    }

                }

                var generic = Listar(_sedeNivelRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDUSUARIONIVEL.ToString(),
                    cell = new string[]
                            {                               
                                "1",
                                item.FLGESTADO==null?"0":item.FLGESTADO,
                                item.IDUSUARIONIVEL==null?"0":item.IDUSUARIONIVEL.ToString(),
                                item.IDUSUARIO==null?"0":item.IDUSUARIO.ToString(),
                                item.IDESEDE==null?"0":item.IDESEDE.ToString(),
                                item.IDEDEPENDENCIA==null?"0":item.IDEDEPENDENCIA.ToString(),
                                item.IDEDEPARTAMENTO==null?"0":item.IDEDEPARTAMENTO.ToString(),
                                item.IDEAREA==null?"0":item.IDEAREA.ToString(),
                                item.SEDEDES==null?"":item.SEDEDES.ToString(),
                                item.DEPENDENCIADES==null?"":item.DEPENDENCIADES.ToString(),
                                item.DEPARTAMENTODES==null?"":item.DEPARTAMENTODES.ToString(),
                                item.AREADES==null?"":item.AREADES.ToString()

   
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
        /// Inicializa el popup de Sedes por cada nivel depencia,departamento,Area
        /// </summary>
        /// <returns></returns>
        public ActionResult popupSedeNivel(string id,string idSel) {

            

            UsuarioRolSedeViewModel usuarioRolSedeViewModel;
            usuarioRolSedeViewModel = new UsuarioRolSedeViewModel();
           

            usuarioRolSedeViewModel = inicializarNivelesSede(Convert.ToInt32(idSel));
            usuarioRolSedeViewModel.SedeNivel = new SedeNivel();
            usuarioRolSedeViewModel.SedeNivel.IDESEDE = Convert.ToInt32(idSel);
            usuarioRolSedeViewModel.SedeNivel.IDUSUARIO = Convert.ToInt32(id);

            return View("popupSedeNivel", usuarioRolSedeViewModel);

        }
        

        /// <summary>
        /// Inicaliza las lista de valores de las lista de valores de la dependecia,departamento,Area
        /// </summary>
        /// <returns></returns>
        public UsuarioRolSedeViewModel inicializarNivelesSede(int idSel)
        {
            var usuarioRolSedeViewModel = new UsuarioRolSedeViewModel();

            usuarioRolSedeViewModel.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                         && x.IdeSede == idSel));
            usuarioRolSedeViewModel.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            usuarioRolSedeViewModel.Departamentos = new List<Departamento>();
            usuarioRolSedeViewModel.Departamentos.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            usuarioRolSedeViewModel.Areas = new List<Area>();
            usuarioRolSedeViewModel.Areas.Add(new Area { IdeArea = 0, NombreArea = "Seleccionar" });

            return usuarioRolSedeViewModel;
        }

        /// <summary>
        /// Graba las sedes con su depencia,departamento,Area
        /// </summary>
        /// <param name="selc"></param>
        /// <param name="idUsu"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetSedeNivel(UsuarioRolSedeViewModel model)
        {
            JsonMessage objJson = new JsonMessage();
            SedeNivel objSedeNivel;
            objSedeNivel = new SedeNivel();
            
            int indGrabo = 0;


            var lista =  _sedeNivelRepository.GetBy(x => x.IDUSUARIO == model.SedeNivel.IDUSUARIO
                                       && x.IDESEDE == model.SedeNivel.IDESEDE
                                       && x.IDEDEPENDENCIA == model.SedeNivel.IDEDEPENDENCIA
                                       && x.IDEDEPARTAMENTO == model.SedeNivel.IDEDEPARTAMENTO
                                       && x.IDEAREA == model.SedeNivel.IDEAREA);

            if (lista.Count==0)
            {
                objSedeNivel = _sedeNivelRepository.GetSingle(x => x.IDESEDE == model.SedeNivel.IDESEDE &&
                                                x.IDUSUARIO == model.SedeNivel.IDUSUARIO);

                objSedeNivel.IDEAREA = model.SedeNivel.IDEAREA;
                objSedeNivel.IDEDEPENDENCIA = model.SedeNivel.IDEDEPENDENCIA;
                objSedeNivel.IDEDEPARTAMENTO = model.SedeNivel.IDEDEPARTAMENTO;

                _sedeNivelRepository.Update(objSedeNivel);
                indGrabo = 1;
            }

            if (indGrabo > 0)
            {
                objJson.Mensaje = "Se registraron los datos correctamente";
                objJson.Resultado = true;
            }
            else
            {
                objJson.Mensaje = "Error, Consulte con el area de sistemas";
                objJson.Resultado = false;
            }

            return Json(objJson); ;
        }

        /// <summary>
        /// Lista de departamentos por dependencia
        /// </summary>
        /// <param name="ideDependencia"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaDepartamentos(int ideDependencia)
        {
            ActionResult result = null;
            Dependencia objDepencia = new Dependencia();

            var listaResultado = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == ideDependencia));

            foreach (Departamento item in listaResultado)
            {
                item.Dependencia = null;
            }
            
            result = Json(listaResultado);
            return result;
        }


        /// <summary>
        /// Lista de areas por departamento
        /// </summary>
        /// <param name="ideDependencia"></param>
        /// <returns></returns>
         [HttpPost]
        public ActionResult listaAreas(int ideDepartamento)
        {
            ActionResult result = null;

            var listaResultado = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento));

            foreach (Area item in listaResultado)
            {
                item.Departamento = null;
            }
             
             result = Json(listaResultado);
            return result;
        }

    }
}
