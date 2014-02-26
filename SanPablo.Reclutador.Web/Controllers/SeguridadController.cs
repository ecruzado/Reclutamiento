using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using NHibernate.Criterion;
using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Entity.Validation;
using SanPablo.Reclutador.Repository.Interface;
using SanPablo.Reclutador.Web.Core;
using SanPablo.Reclutador.Web.Models;
using SanPablo.Reclutador.Web.Models.JQGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SanPablo.Reclutador.Web.Controllers
{
    [AllowAnonymous]
    public class SeguridadController : BaseController
    {
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

        public ActionResult Login() 
        {
            logout();
            return View();
        }

        /// <summary>
        /// Cierra la sesion del usuario
        /// </summary>
        private void logout()
        {

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
        /// Logeo de usuario Extranet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Logon(SeguridadViewModel model)
        {
            JsonMessage ObjJsonMessage = new JsonMessage();
            Usuario objUSaurioExtranet;
            string codUsuario = model.UsuarioExtranet.Usuario;
            string PassUsuario = model.UsuarioExtranet.Password;

            var ListaUsuario = (List<Usuario>)_usuarioRepository.GetBy(x => x.CodUsuario == codUsuario
                                    && x.CodContrasena == PassUsuario
                                    && x.TipUsuario == TipUsuario.Extranet
                                    && x.FlgEstado == IndicadorActivo.Activo
                                    );

            if (ListaUsuario != null && ListaUsuario.Count == 1)
            {

                foreach (Usuario item in ListaUsuario)
                {
                    ObjJsonMessage.Resultado = true;
                    objUSaurioExtranet = item;
                    Session[ConstanteSesion.ObjUsuarioExtranet] = objUSaurioExtranet;     
                }
                
            }
            else
            {
                ObjJsonMessage.Resultado = false;
                ObjJsonMessage.Mensaje = "Usuario o contraseña incorrecta";
            }

            return Json(ObjJsonMessage);
        }
        /// <summary>
        /// Inicializa el registro de un nuevo usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult InicializaPopupExtranet(){

            SeguridadViewModel model;

            model = new SeguridadViewModel();
            model.UsuarioExtranet = new UsuarioExtranet();

            return View("PopupLogeo",model);
        }


        public ActionResult LogeoUsuario(SeguridadViewModel model){

            
            JsonMessage objJson;
            objJson = new JsonMessage();
            Usuario objUsuarioExtranet;
            Usuario objUsuarioIntranet;
            string usuario=null;
            string pass=null;
            string confPass=null;

            


            if (model != null)
            { 

                UsuarioExtranetValidator validator = new UsuarioExtranetValidator();
                ValidationResult result = validator.Validate(model.UsuarioExtranet, "Usuario", "Password");
                JsonMessage objJsonMensage = new JsonMessage();
                

                if (!result.IsValid)
                {
                    objJson.Resultado = false;
                    objJson.Mensaje = "los campos (*) son obligatorios";
                    
                    return Json(objJsonMensage);
                }
                
                
                
                usuario = model.UsuarioExtranet.Usuario;
                pass = model.UsuarioExtranet.Password;
                confPass = model.UsuarioExtranet.PasswordConfirma;

                var lista = _usuarioRepository.GetBy(x => x.CodUsuario == usuario
                                                    && x.CodContrasena == pass
                                                    && x.TipUsuario == TipUsuario.Extranet
                                                    );

                if (lista != null && lista.Count>0)
                {
                    objJson.Resultado = false;
                    objJson.Mensaje = "El usuario ya se encuentra registrado, Ingrese un nuevo usuario";

                }
                else
                {
                    
                    
                    objUsuarioExtranet = new Usuario();
                    objUsuarioExtranet.CodUsuario = usuario;
                    objUsuarioExtranet.CodContrasena = pass;
                    objUsuarioExtranet.Email = usuario;
                    objUsuarioExtranet.TipUsuario = TipUsuario.Extranet;
                    objUsuarioExtranet.UsrCreacion = UsuarioInicio.Web;
                    objUsuarioExtranet.FecCreacion = FechaCreacion;
                    objUsuarioExtranet.FlgEstado = IndicadorActivo.Activo;

                    _usuarioRepository.Add(objUsuarioExtranet);
                    
                    objUsuarioIntranet =new Usuario();
                    objUsuarioIntranet.CodUsuario = usuario;
                    objUsuarioIntranet.CodContrasena = pass;
                    objUsuarioIntranet.Email = usuario;
                    objUsuarioIntranet.TipUsuario = TipUsuario.Instranet;
                    objUsuarioIntranet.UsrCreacion = UsuarioInicio.Web;
                    objUsuarioIntranet.FecCreacion = FechaCreacion;
                    objUsuarioIntranet.FlgEstado = IndicadorActivo.Inactivo;
                    
                    _usuarioRepository.Add(objUsuarioIntranet);

                    UsuarioRolSede objUsuarioRolSede;
                    objUsuarioRolSede = new UsuarioRolSede();

                    objUsuarioRolSede.IdSede = 0;
                    objUsuarioRolSede.IdUsuario = objUsuarioIntranet.IdUsuario;

                    var objRol = _rolRepository.GetSingle(x => x.CodRol.Upper() == UsuarioInicio.POSTULANTE.Upper());

                    objUsuarioRolSede.IdRol = objRol.IdRol;
                    objUsuarioRolSede.UsuarioCreacion = UsuarioInicio.Web;
                    objUsuarioRolSede.FechaCreacion = FechaCreacion;

                    _usuarioRolSedeRepository.Add(objUsuarioRolSede);

                    objJson.Resultado = true;
                    objJson.Mensaje = "Se registro el usuario correctamente";

                }


            }
            else {
                objJson.Resultado = false;
                objJson.Mensaje = "Error en registrar el usuario";
            }


            return Json(objJson);
        }

        /// <summary>
        /// Inicializa el popup para restablecer la constraseña, no la cambia la restablece
        /// </summary>
        /// <returns></returns>
        public ActionResult InicializaPopupRestablePass() 
        {
            SeguridadViewModel model;

            model = new SeguridadViewModel();
            model.UsuarioExtranet = new UsuarioExtranet();

            return View("PopupRestablecePass",model);
        }


        /// <summary>
        /// Envia la contraseña al correo del usuario externo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RestablecePass(SeguridadViewModel model) 
        {
            JsonMessage objJsonMensaje = new JsonMessage();
            string codUsuario;
            try
            {
                if (model != null)
                {
                   var objUsuario =  _usuarioRepository.GetSingle(x => x.CodUsuario == model.UsuarioExtranet.Usuario
                                                && x.TipUsuario == TipUsuario.Extranet);

                   if (objUsuario!=null)
                   {
                       codUsuario = objUsuario.Email;
                       //envia Emial;
                       objJsonMensaje.Resultado = true;
                       objJsonMensaje.Mensaje = "Se envio la contraseña a su correo electrónico";

                   }
                }
                else
                {
                    objJsonMensaje.Resultado = false;
                    objJsonMensaje.Mensaje = "Error : comuniquese con el area de sistemas";
                }
            }
            catch (Exception)
            {
                objJsonMensaje.Resultado = false;
                objJsonMensaje.Mensaje = "Error : comuniquese con el area de sistemas";
            }
          

            return Json(objJsonMensaje);


        }

        /// <summary>
        /// obtiene los menus del Extranet
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenuEx()
        {

            //FormsAuthentication.SetAuthCookie(id, false);
            Usuario usuario;
            var objUsuario = Session[ConstanteSesion.ObjUsuarioExtranet];

            if (objUsuario!=null)
            {
                usuario = (Usuario)objUsuario;
            }

            var objRol = _rolRepository.GetSingle(x => x.CodRol.Upper() == UsuarioInicio.POSTULANTE.Upper());
            
            SeguridadViewModel objModel = new SeguridadViewModel();

            List<MenuItem> ListaItem = new List<MenuItem>(_rolOpcionRepository.GetMenu(Convert.ToInt32(objRol.IdRol), TipMenu.Extranet));
            objModel.listaMenu = (ListaItem.Where(n => n.TIPMENU == TipMenu.Extranet).ToList());

            List<MenuPadre> ListaPadre = new List<MenuPadre>(_rolOpcionRepository.GetMenuPadre(Convert.ToInt32(objRol.IdRol), TipMenu.Extranet));
            objModel.listaPadre = (ListaPadre.Where(n => n.TIPMENU == TipMenu.Extranet).ToList());

            if (objModel.listaPadre != null && objModel.listaPadre.Count>0)
            {
                Session["ListaMenu"] = objModel.listaMenu;
                Session["listaPadre"] = objModel.listaPadre;
                
                return RedirectToAction("General", "Postulante");
            }
            else
            {
                return RedirectToAction("Login", "Seguridad");
            }
           
        }

    }
}
