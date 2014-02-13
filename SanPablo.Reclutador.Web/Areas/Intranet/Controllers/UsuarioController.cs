

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
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;

        public UsuarioController(IRolRepository rolRepository, IDetalleGeneralRepository detalleGeneralRepository,
                             IRolOpcionRepository rolOpcionRepository, IUsuarioRepository usuarioRepository,
                             IUsuarioRolSedeRepository usuarioRolSedeRepository)
        {
            _rolRepository = rolRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _rolOpcionRepository = rolOpcionRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
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


        /// <summary>
        /// Editar obtiene los campos de los valores para editar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string id)
        {

            UsuarioViewModel model = new UsuarioViewModel();
            model.Usuario = new Usuario();
            
            var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(id));

            model.Usuario = objUsuario;

            model.Accion = Accion.Editar;

            return View("Edit", model);
        }


        [HttpPost]
        public ActionResult Redirec(string id)
        {
            return RedirectToAction("Edit", "Usuario", new { id = id });
            
        }
        
       

         //{ name: 'IdUsuarolSede', index: 'IdUsuarolSede', align: 'left', editable: false, sortable: false, hidden: true },
         //            { name: 'IdSede', index: 'IdSede', align: 'left', editable: false, sortable: false, hidden: true },
         //            { name: 'DescSede', index: 'DescSede', align: 'left', sortable: false, editable: false, width: 350 },
         //            { name: 'IdUsuario', index: 'IdUsuario', align: 'left', sortable: false, editable: false, hidden: true },
         //            { name: 'IdRol', index: 'IdRol', align: 'left', sortable: false,  editable: false, hidden: true },
         //            { name: 'DescRol', index: 'DescOpcion', align: 'left', sortable: false, width: 350, editable: false }


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
                                "1",
                                item.IdUsuario==null?"":item.IdUsuario.ToString(),
                                item.IdRol==null?"":item.IdRol.ToString(),
                                "1"
                    
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


    }
}
