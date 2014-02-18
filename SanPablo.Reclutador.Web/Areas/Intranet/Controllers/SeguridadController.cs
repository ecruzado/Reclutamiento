

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

        public SeguridadController(IRolRepository rolRepository, IDetalleGeneralRepository detalleGeneralRepository,
                             IRolOpcionRepository rolOpcionRepository, IUsuarioRepository usuarioRepository,
                             IUsuarioRolSedeRepository usuarioRolSedeRepository,
                             ISedeRepository sedeRepository,
                             IUsuarioVistaRepository usuarioVistaRepository
                                )
        {
            _rolRepository = rolRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _rolOpcionRepository = rolOpcionRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _sedeRepository = sedeRepository;
            _usuarioVistaRepository = usuarioVistaRepository;
        }


        public ActionResult Login()
        {
            SeguridadViewModel model = new SeguridadViewModel();
            model.Accion = Accion.Nuevo;
            model.listaRol = new List<Rol>();
            model.listaSede = new List<Sede>();

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

                objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id);
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

        private SeguridadViewModel InicializarRol(Usuario objUsuario)
        {
            var objModel = new SeguridadViewModel();
            objModel.Usuario = new Usuario();

            objModel.listaRol = new List<Rol>(_usuarioRolSedeRepository.GetListaRol(objUsuario.IdUsuario));

            if (objModel.listaRol.Count>0)
            {
                objModel.Accion = Accion.Editar;
            }
            else
            {
                objModel.Accion = Accion.Nuevo;
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

                objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id);
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

                objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id);
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

            if (String.IsNullOrEmpty(id) )
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
                objJsonMensaje.Mensaje = "Seleccione un roll";
                return Json(objJsonMensaje);
            }

            if (Convert.ToInt32(codRol)==0)
            {
                objJsonMensaje.Mensaje = "Seleccione un roll";
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


            objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id.Trim() 
                                         && x.CodContrasena == codPass.Trim());

            if (objUsuario!=null)
            {
                if (objUsuario.IdUsuario!=null)
                {
                    objJsonMensaje.Resultado = true;
                    objJsonMensaje.IdDato = Convert.ToInt32(codRol);
                }
            }

            
            return Json(objJsonMensaje);

        }


        /// <summary>
        /// obtiene el el menu del usuario por el rol
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetMenu(string id) {

            SeguridadViewModel objModel = new SeguridadViewModel();

            JsonMessage objMessage = new JsonMessage();
            MenuItem objMenuItem = new MenuItem();

            objModel.listaMenu =new List<MenuItem>(_rolOpcionRepository.GetMenu(Convert.ToInt32(id)));
            objModel.listaPadre = new List<MenuPadre>(_rolOpcionRepository.GetMenuPadre(Convert.ToInt32(id)));

            Session["ListaMenu"] = objModel.listaMenu;
            Session["listaPadre"] = objModel.listaPadre;
            
            return RedirectToAction("ListaReemplazo", "SolicitudCargo");

        
        }


    }
}
