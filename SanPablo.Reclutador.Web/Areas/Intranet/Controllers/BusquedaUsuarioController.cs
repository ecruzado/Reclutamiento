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
               
                DetachedCriteria where = null;
                where = DetachedCriteria.For<Usuario>();
                
                if(
                    (!"".Equals(grid.rules[1].data) && grid.rules[1].data != null && grid.rules[1].data != "0") ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0") ||
                    (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0") 
                   )
                {

                    if (!"".Equals(grid.rules[1].data) && grid.rules[1].data != null && "0"!=(grid.rules[1].data))
                    {

                        where.Add(Expression.Like("DscApePaterno", '%' + grid.rules[1].data + '%'));
                    }
                    if (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && "0"!=(grid.rules[2].data))
                    {
                        where.Add(Expression.Like("DscApeMaterno", '%' + grid.rules[2].data + '%'));
                    }
                    if (!"".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                    {
                        where.Add(Expression.Like("DscNombres", '%' + grid.rules[3].data + '%'));
                    }
                }

                where.Add(Expression.Eq("FlgEstado", IndicadorActivo.Activo));
                where.Add(Expression.Eq("TipUsuario", TipUsuario.Instranet));

                var generic = Listar(_usuarioRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdUsuario.ToString(),
                    cell = new string[]
                            {
                                item.IdUsuario==0?"":item.IdUsuario.ToString(),
                                item.DscApePaterno==null?"":item.DscApePaterno.ToString(),
                                item.DscApeMaterno==null?"":item.DscApeMaterno.ToString(),
                                item.DscNombres==null?"":item.DscNombres.ToString()
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
