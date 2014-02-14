

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


    public class UsuarioController : BaseController
    {
       
        private IRolRepository _rolRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IRolOpcionRepository _rolOpcionRepository;
        private IUsuarioRepository _usuarioRepository;

        public UsuarioController(IRolRepository rolRepository, IDetalleGeneralRepository detalleGeneralRepository,
                             IRolOpcionRepository rolOpcionRepository, IUsuarioRepository usuarioRepository)
        {
            _rolRepository = rolRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _rolOpcionRepository = rolOpcionRepository;
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// inicializa la pantalla inicial de usuarios
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Crea un nuevo Usuario
        /// </summary>
        /// <returns></returns>
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
        public ActionResult valCodUsuario(string id)
        {
            UsuarioViewModel usuModel = new UsuarioViewModel();
            JsonMessage jsonMessage = new JsonMessage();

            var objUsuario = _usuarioRepository.GetSingle(x => x.CodUsuario == id);

            if (objUsuario!=null)
            {
                jsonMessage.Resultado = false;
                jsonMessage.Mensaje = "El código de usuario ya se encuentra registrado por favor registrar uno diferente";
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
        



    }
}
