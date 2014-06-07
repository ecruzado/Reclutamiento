

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
    using System.Web.Security;

    
    public class SeguridadController : BaseController
    {
        //
        // GET: /Intranet/Seguridad/

        private IRolRepository _rolRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IRolOpcionRepository _rolOpcionRepository;
        private IUsuarioRepository _usuarioRepository;
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;
        private ISedeRepository _sedeRepository;
        private IUsuarioVistaRepository _usuarioVistaRepository;
        private ISedeNivelRepository _sedeNivelRepository;

        public SeguridadController(IRolRepository rolRepository, IDetalleGeneralRepository detalleGeneralRepository,
                             IRolOpcionRepository rolOpcionRepository, IUsuarioRepository usuarioRepository,
                             IUsuarioRolSedeRepository usuarioRolSedeRepository,
                             ISedeRepository sedeRepository,
                             IUsuarioVistaRepository usuarioVistaRepository,
                             ISedeNivelRepository sedeNivelRepository
                                )
        {
            _rolRepository = rolRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _rolOpcionRepository = rolOpcionRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _sedeRepository = sedeRepository;
            _usuarioVistaRepository = usuarioVistaRepository;
            _sedeNivelRepository = sedeNivelRepository;
        }

        /// <summary>
        /// Cierra Session
        /// </summary>
        private void logout() {

            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            HttpCookie cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
        
        }

        /// <summary>
        /// Reliza el logeo de Intranet
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {


            logout();
            
            
            SeguridadViewModel model = new SeguridadViewModel();
            model.Accion = Accion.Nuevo;
            model.listaRol = new List<Rol>();
            model.listaSede = new List<Sede>();

            model.listaRol.Insert(0, new Rol { IdRol = 0, CodRol = "Seleccionar" });

            model.Usuario = new Usuario();
            model.Rol = new Rol();
            model.Sede = new Sede();
            return View(model);
        }


        /// <summary>
        /// obtiene el rol del usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult getRol(string id)
        {
            JsonMessage objJsonMensaje = new JsonMessage();
            Usuario objUsuario = new Usuario();
            SeguridadViewModel model = new SeguridadViewModel();

            if (id != null)
            {

                objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id && x.TipUsuario == TipUsuario.Instranet && x.FlgEstado==IndicadorActivo.Activo);
                if (objUsuario!=null)
                {

                    model = InicializarRol(objUsuario);
                    
                }
            }

            objJsonMensaje.Objeto = model.listaRol;
            objJsonMensaje.Resultado = true;
            //return Json(objJsonMensaje);
            return Json(model.listaRol);
            
        }

        /// <summary>
        /// inicializa los roles
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        private SeguridadViewModel InicializarRol(Usuario objUsuario)
        {
            var objModel = new SeguridadViewModel();
            objModel.Usuario = new Usuario();

            objModel.listaRol = new List<Rol>(_usuarioRolSedeRepository.GetListaRol(objUsuario.IdUsuario));

            if (objModel.listaRol.Count>0)
            {
                int dato = 1;
            }
            else
            {
                objModel.listaRol = new List<Rol>();
            }
            
            objModel.listaRol.Insert(0, new Rol { IdRol = 0, CodRol = "Seleccionar" });

            objModel.Rol = new Rol();

            return objModel;
        }


        /// <summary>
        /// valida que existan sedes para el usuario y el rol
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codRol"></param>
        /// <returns></returns>
        public ActionResult validaSede(string id, string codRol)
        {
            JsonMessage objJsonMensaje = new JsonMessage();
            Usuario objUsuario = new Usuario();
            SeguridadViewModel model = new SeguridadViewModel();

            if (id != null)
            {

                objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id && x.TipUsuario == TipUsuario.Instranet && x.FlgEstado == IndicadorActivo.Activo);
                if (objUsuario != null)
                {

                    model = InicializarSede(objUsuario, codRol);

                }
            }
            if (Visualicion.SI.Equals(model.Visualicion))
            {
                objJsonMensaje.Objeto = model.listaSede;
                objJsonMensaje.Resultado = true;
            }
            else
            {
                objJsonMensaje.Resultado = false;
            }
           
           
            return Json(objJsonMensaje);

        }


        /// <summary>
        /// Obtiene las sede por usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult getSede(string id,string codRol)
        {
            JsonMessage objJsonMensaje = new JsonMessage();
            Usuario objUsuario = new Usuario();
            SeguridadViewModel model = new SeguridadViewModel();

            if (id != null)
            {

                objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id && x.FlgEstado == IndicadorActivo.Activo && x.TipUsuario == TipUsuario.Instranet);
                if (objUsuario != null)
                {

                    model = InicializarSede(objUsuario, codRol);

                }
            }

            objJsonMensaje.Objeto = model.listaSede;
            objJsonMensaje.Resultado = true;
            //return Json(objJsonMensaje);
            return Json(model.listaSede);

        }

        /// <summary>
        /// Inicializa las Sede del Usuario
        /// </summary>
        /// <param name="objUsuario"></param>
        /// <returns></returns>
        private SeguridadViewModel InicializarSede(Usuario objUsuario, string codRol)
        {
            var objModel = new SeguridadViewModel();
            objModel.Usuario = new Usuario();

            objModel.listaSede = new List<Sede>(_usuarioRolSedeRepository.GetListaSede(objUsuario.IdUsuario, Convert.ToInt32(codRol)));

            if (objModel.listaSede.Count > 0)
            {
                objModel.Accion = Accion.Editar;
                objModel.Visualicion = Visualicion.SI;
            }
            else
            {
                objModel.Accion = Accion.Nuevo;
                objModel.Visualicion = Visualicion.NO;
                objModel.listaSede = new List<Sede>();
            }

            objModel.listaSede.Insert(0, new Sede { CodigoSede = "0", DescripcionSede = "Seleccionar" });

            objModel.Sede = new Sede();

            return objModel;
        }


        /// <summary>
        /// valida el logeo del usaurio
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Logon(string id, string codPass, string codRol, string codSede, string indSede)
        {
            JsonMessage objJsonMensaje = new JsonMessage();
            Usuario objUsuario = new Usuario();
            SeguridadViewModel model = new SeguridadViewModel();
            objJsonMensaje.Resultado = false;

            int reqSede = Convert.ToInt32(indSede);

            if (String.IsNullOrEmpty(id))
            {
                objJsonMensaje.Mensaje = "Ingrese un usuario";
                return Json(objJsonMensaje);
            }

            if (String.IsNullOrEmpty(codPass))
            {
                objJsonMensaje.Mensaje = "Ingrese un password";
                return Json(objJsonMensaje);
            }

            if (String.IsNullOrEmpty(codRol))
            {
                objJsonMensaje.Mensaje = "Seleccione un rol";
                return Json(objJsonMensaje);
            }

            if (Convert.ToInt32(codRol) == 0)
            {
                objJsonMensaje.Mensaje = "Seleccione un rol";
                return Json(objJsonMensaje);
            }


            if (Convert.ToInt32(codRol) > 0 && reqSede > 0)
            {
                if (String.IsNullOrEmpty(codSede))
                {
                    objJsonMensaje.Mensaje = "Seleccione una sede";
                    return Json(objJsonMensaje);
                }
                if ("0".Equals(codSede))
                {
                    objJsonMensaje.Mensaje = "Seleccione una sede";
                    return Json(objJsonMensaje);
                }
            }


            string password = Base64Encode(codPass.Trim());

            objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id.Trim()
                                         && x.CodContrasena == password
                                         && x.TipUsuario == TipUsuario.Instranet
                                         && x.FlgEstado == IndicadorActivo.Activo);

            var objRol = _rolRepository.GetSingle(x => x.IdRol == Convert.ToInt32(codRol));


            if (objUsuario != null)
            {
                if (objUsuario.IdUsuario != null)
                {
                    objJsonMensaje.Resultado = true;
                    objJsonMensaje.IdDato = Convert.ToInt32(codRol);

                    Session[Core.ConstanteSesion.Usuario] = objUsuario.IdUsuario;
                    Session[Core.ConstanteSesion.UsuarioDes] = objUsuario.CodUsuario;

                    Session[Core.ConstanteSesion.Rol] = codRol;
                    Session[Core.ConstanteSesion.RolDes] = objRol.DscRol;

                    Session[Core.ConstanteSesion.UsuarioSede] = null;

                    Session[Core.ConstanteSesion.ObjUsuario] = objUsuario;

                    if (codSede != null && !"".Equals(codSede.Trim()) && !"0".Equals(codSede.Trim()))
                    {
                        var objSede = _sedeRepository.GetSingle(x => x.CodigoSede == codSede);

                        Session[Core.ConstanteSesion.Sede] = codSede;
                        Session[Core.ConstanteSesion.SedeDes] = objSede.DescripcionSede;

                        var objSedeNivel = _sedeNivelRepository.GetSingle(x => x.IDESEDE == Convert.ToInt32(codSede)
                                                       && x.IDUSUARIO == objUsuario.IdUsuario);

                        Session[Core.ConstanteSesion.UsuarioSede] = objSedeNivel;
                    }
                    else
                    {   //obtiene la sede para el postulante se maneja de uno a uno
                        if (TipUsuario.Instranet.Equals(objUsuario.TipUsuario))
                        {
                            try
                            {
                                var ObjRol = _rolRepository.GetSingle(x=> x.IdRol == Convert.ToInt32(codRol) && x.FlgEstado==IndicadorActivo.Activo);
                                if (ObjRol!=null)
                                {
                                    if (Indicador.No.Equals(ObjRol.FlgSede))
	                                {
                                        int dato = 0;
	                                }
                                }
                               
                            }
                            catch (Exception)
                            {

                                objJsonMensaje.Resultado = false;
                                objJsonMensaje.Mensaje = "Error: Se econtro más de una sede configurada";
                            }
                        }


                    }

                }
            }
            else
            {
                objJsonMensaje.Resultado = false;
                objJsonMensaje.Mensaje = "Usuario o contraseña incorrecta";
            }

            return Json(objJsonMensaje);

        }



        /// <summary>
        /// obtiene el el menu del usuario por el rol
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetMenu(string id) {

            FormsAuthentication.SetAuthCookie(id, false);
            SeguridadViewModel objModel = new SeguridadViewModel();

            JsonMessage objMessage = new JsonMessage();
            MenuItem objMenuItem = new MenuItem();


            List<MenuItem> ListaItem = new List<MenuItem>(_rolOpcionRepository.GetMenu(Convert.ToInt32(id), TipMenu.Instranet));
            objModel.listaMenu = (ListaItem.Where(n => n.TIPMENU == TipMenu.Instranet).ToList());

            List<MenuPadre> ListaPadre = new List<MenuPadre>(_rolOpcionRepository.GetMenuPadre(Convert.ToInt32(id), TipMenu.Instranet));
            objModel.listaPadre = (ListaPadre.Where(n => n.TIPMENU == TipMenu.Instranet).ToList());

            if (objModel.listaPadre!=null)
            {
                Session["ListaMenu"] = objModel.listaMenu;
                Session["listaPadre"] = objModel.listaPadre;
               
                //return RedirectToAction("ListaReemplazo", "SolicitudCargo");
                return RedirectToAction("Inicio", "Seguridad");
            }
            else
            {
                return RedirectToAction("Login", "Seguridad");
            }
           
        }

        /// <summary>
        /// pagina inicial
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Inicio() 
        { 
            SeguridadViewModel model = new SeguridadViewModel();

            return View("Inicio", model);
        
        }


        /// <summary>
        /// validacion del nuevo password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
          
         [HttpPost]
         [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
         public ActionResult ResetPass(UsuarioRolSedeViewModel model) {

             JsonMessage ObjJsonMessage;
             ObjJsonMessage = new JsonMessage();
             model.Usuario = new Usuario();
             string passwordAnt;
             string passwordNuevo;

             var codUsuario = Session[ConstanteSesion.Usuario];

             if (codUsuario != null)
             {

                 model.Usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(codUsuario));

                 if (model.Usuario != null)
                 {
                     passwordAnt = Base64Encode(model.Password.PassAnterior.Trim());
                     
                     if (model.Usuario.CodContrasena.Equals(passwordAnt))
                     {

                         passwordNuevo = Base64Encode(model.Password.PassNuevo);

                         model.Usuario.CodContrasena = passwordNuevo;
                        _usuarioRepository.Update(model.Usuario);
                         ObjJsonMessage.Resultado = true;
                     }
                     else
                     {
                         ObjJsonMessage.Resultado = false;
                         ObjJsonMessage.Mensaje = "La contraseña anterior no existe";
                     }
                 }

             }
             
             return Json(ObjJsonMessage);

        }


        


    }
}
