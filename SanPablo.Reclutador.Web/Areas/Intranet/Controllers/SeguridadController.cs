

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

            model.Usuario = new Usuario();
            model.Rol = new Rol();
            return View(model);
        }


        /// <summary>
        /// obtiene el rol del usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

       
        //public ActionResult getRol(string model)
        //{
        //    JsonMessage objJsonMensaje = new JsonMessage();
        //    Usuario objUsuario = new Usuario();



        //    return View();
        //}


       
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

                    model = Inicializar(objUsuario);
                    
                }
            }

            objJsonMensaje.Objeto = model.listaRol;
            objJsonMensaje.Resultado = true;
            //return Json(objJsonMensaje);
            return Json(model.listaRol);
            
        }

        private SeguridadViewModel Inicializar(Usuario objUsuario)
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

      


    }
}
