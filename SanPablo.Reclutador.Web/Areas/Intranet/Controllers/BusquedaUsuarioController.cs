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

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{

    [Authorize]
    public class BusquedaUsuarioController : BaseController
    {
       
        private IRolRepository _rolRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IUsuarioRepository _usuarioRepository;
        private ISedeRepository _sedeRepository;

        public BusquedaUsuarioController(IRolRepository rolRepository,
                                         IDetalleGeneralRepository detalleGeneralRepository,
                                         IUsuarioRepository usuarioRepository,
                                         ISedeRepository sedeRepository)
        {
            _rolRepository = rolRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _usuarioRepository = usuarioRepository;
            _sedeRepository = sedeRepository;
        }

        /// <summary>
        /// inicializa la pantalla inicial de usuarios
        /// </summary>
        /// <returns></returns>
        
        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult Index()
        {
            UsuarioViewModel modelUsuario = InicializarPopupUsuario();
            modelUsuario.Usuario = new Usuario();

            return View("PopupUsuario", modelUsuario);
        }

        /// <summary>
        /// Iniciar el Pop -up
        /// </summary>
        /// <returns></returns>
        public UsuarioViewModel InicializarPopupUsuario()
        {
            UsuarioViewModel model = new UsuarioViewModel();

            model.Usuario = new Usuario();
            model.Roles = new List<Rol>(_rolRepository.GetBy(x => x.FlgEstado == IndicadorActivo.Activo));
            model.Roles.Insert(0, new Rol { IdRol = 0, DscRol = "Seleccione" });

            return model;
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
                var ideSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
                var usuarioResponsable = new Usuario();
                
                usuarioResponsable.DscApePaterno = (grid.rules[1].data == null ? "":(grid.rules[1].data).ToString());
                usuarioResponsable.DscApeMaterno = (grid.rules[2].data == null ? "":(grid.rules[2].data).ToString());
                usuarioResponsable.DscNombres = (grid.rules[3].data == null ? "":(grid.rules[3].data).ToString());
                usuarioResponsable.IdRol = (grid.rules[4].data == null ? 0 : Convert.ToInt32(grid.rules[4].data));
               
                var lista = _usuarioRepository.listarUsuario(usuarioResponsable,ideSede);


                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdUsuario.ToString()+item.IdRol.ToString(),
                    cell = new string[]
                            {
                                item.IdUsuario==0?"":item.IdUsuario.ToString(),
                                item.DscApePaterno==null?"":item.DscApePaterno.ToString(),
                                item.DscApeMaterno==null?"":item.DscApeMaterno.ToString(),
                                item.DscNombres==null?"":item.DscNombres.ToString(),
                                item.IdRol == null?"":item.IdRol.ToString(),
                                item.Rol == null?"":item.Rol.ToString()
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
