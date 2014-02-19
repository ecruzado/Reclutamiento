
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
    public class RolController : BaseController
    {
        private IRolRepository _rolRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IRolOpcionRepository _rolOpcionRepository;

        public RolController(IRolRepository rolRepository,IDetalleGeneralRepository detalleGeneralRepository,
                             IRolOpcionRepository rolOpcionRepository)
        {
            _rolRepository = rolRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _rolOpcionRepository = rolOpcionRepository;
        }

        /// <summary>
        /// Inicializa la view
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        public ActionResult Index()
        {

            RolViewModel rolModel = new RolViewModel();
            rolModel.rol = new Rol();
            rolModel = InicializarRolEdit();
            return View("Index", rolModel);

        }

        //public ActionResult Edit()
        //{
        //    return View();
        //}

        /// <summary>
        /// Iniaciliza la pag para crear un nuevo Rol
        /// </summary>
        /// <returns></returns>
         [ValidarSesion]
        public ActionResult Nuevo()
        {
            RolViewModel rolModel = new RolViewModel();
           
            rolModel.rol = new Rol();
            rolModel = InicializarRolEdit();
            rolModel.Accion = Accion.Nuevo;
           
            return View("Edit", rolModel);
        }

        /// <summary>
        /// Inicializa la lista de valores de la pagina
        /// </summary>
        /// <returns></returns>
        private RolViewModel InicializarRolEdit()
        {
            var rolViewModel = new RolViewModel();
            rolViewModel.rol = new Rol();

            rolViewModel.listaIndSede =
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSede));
            rolViewModel.listaIndSede.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return rolViewModel;
        }

        /// <summary>
        /// Reliza el proceso de crear el rol
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Edit(RolViewModel model)
        {
            JsonMessage objJson = new JsonMessage();
            DateTime hoy = DateTime.Today;

            if (model.rol!=null && model.rol.IdRol != 0)
            {
                var objRrol = _rolRepository.GetSingle(x => x.IdRol == model.rol.IdRol);
                objRrol.FlgSede = model.rol.FlgSede;
                objRrol.FechaModificacion = hoy;
                objRrol.UsuarioModificacion = UsuarioActual.NombreUsuario;
                objRrol.CodRol = model.rol.CodRol;
                objRrol.DscRol = model.rol.DscRol;
                _rolRepository.Update(objRrol);

                var objModel=InicializarRolEdit();
                objModel.rol = objRrol;
                objModel.Accion = Accion.Editar;

                return RedirectToAction("Edicion", "Rol", new { id = objModel.rol.IdRol });
                //return View("Edit", objModel);
               

            }
            else
            {
                //nuevo
                model.rol.UsuarioCreacion = UsuarioActual.NombreUsuario;
                model.rol.FechaCreacion = hoy;
                model.rol.FlgEstado = "A";
                _rolRepository.Add(model.rol);
               
                var objModel = InicializarRolEdit();

                objModel.rol = model.rol;
                objModel.Accion = Accion.Editar;
               
                //return View("Edit", objModel);
                return RedirectToAction("Edicion", "Rol", new { id = objModel.rol.IdRol });
            }
           
        }

        /// <summary>
        /// Muestra las opciones seleccionadas para el rol
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaOpciones(GridTable grid)
        {

            RolOpcion rs = new RolOpcion();
            try
            {
                DetachedCriteria where = null;

                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)))
                {
                    where = DetachedCriteria.For<RolOpcion>();

                    if (!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data))
                    {
                        int dato =  Convert.ToInt32(grid.rules[0].data);
                        where.Add(Expression.Eq("IDROL", dato));
                    }
                    
                }

                var generic = Listar(_rolOpcionRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDROLOPCION.ToString(),
                    cell = new string[]
                            {
                               
                                item.IDROLOPCION==null?"":item.IDROLOPCION.ToString(),
                                item.IDROL==null?"":item.IDROL.ToString(),
                                item.IDOPCION==null?"":item.IDOPCION.ToString(),
                                item.NombreOpcion==null?"":item.NombreOpcion,
                                item.DescOpcion==null?"":item.DescOpcion
                                
                   
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
        /// Lista de roles
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult getListaRol(GridTable grid)
        {
            try
            {
                DetachedCriteria where = null;

                if ((!"".Equals(grid.rules[0].data) && grid.rules[0].data!=null) ||
                    (!"".Equals(grid.rules[1].data) && grid.rules[1].data!=null) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0")
                   )
                {
                    where = DetachedCriteria.For<Rol>();

                    if (!"".Equals(grid.rules[0].data) && grid.rules[0].data!=null)
                    {
                        where.Add(Expression.Like("CodRol", '%' + grid.rules[0].data + '%'));
                    }
                    if (!"".Equals(grid.rules[1].data) && grid.rules[1].data!=null)
                    {
                        where.Add(Expression.Like("DscRol", '%' + grid.rules[1].data + '%'));
                    }
                    if (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0")
                    {
                        where.Add(Expression.Eq("FlgSede", grid.rules[2].data ));
                    }
                   
                }

                var generic = Listar(_rolRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdRol.ToString(),
                    cell = new string[]
                            {
                                item.IdRol==null?"":item.IdRol.ToString(),
                                item.CodRol==null?"":item.CodRol,
                                item.DscRol==null?"":item.DscRol,
                                item.FlgSede==null?"":item.FlgSede,
                                item.DescSede==null?"":item.DescSede
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
        /// Edita el rol
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Edicion(string id)
        {

            RolViewModel model = new RolViewModel();
            model.rol = new Rol();

            model = InicializarRolEdit();
            
            var objRol = _rolRepository.GetSingle(x => x.IdRol == Convert.ToInt32(id));

            model.rol.IdRol = objRol.IdRol;
            model.rol.FlgEstado = objRol.FlgEstado;
            model.rol.FlgSede = objRol.FlgSede;
            model.rol.CodRol = objRol.CodRol;
            model.rol.DscRol = objRol.DscRol;

            model.Accion = Accion.Editar;

            return View("Edit", model);
        }

        /// <summary>
        /// Elimina el rol seleccionado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult EliminarRol(string id)
        {
            
            int resultado=0;
            JsonMessage objJsonMessage = new JsonMessage();

            RolViewModel model = new RolViewModel();
            model.rol = new Rol();

            resultado = _rolRepository.EliminaRol(Convert.ToInt32(id));
            if (resultado>0)
            {
                objJsonMessage.Resultado = true;

                objJsonMessage.Mensaje = "Se elemino el rol correctamente";
            }
            else
            {
                objJsonMessage.Resultado = true;
                objJsonMessage.Mensaje = "El rol no se puede eliminar";
            }

            return Json(objJsonMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codCat"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        public ActionResult EliminarOpcion(string id, string codOp)
        {
            var jsonMessage = new JsonMessage();
            jsonMessage.Resultado = true;
            int retorno=0;
            try
            {
                 retorno = _rolOpcionRepository.EliminaOpcion(Convert.ToInt32(id),Convert.ToInt32(codOp));
                 if (retorno>0)
                 {
                     jsonMessage.Resultado = true;
                     
                 }

            }
            catch (Exception)
            {

                jsonMessage.Resultado = false;
                jsonMessage.Mensaje = "Error : No se permite eliminar la opción";
            }

            return Json(jsonMessage);
        }

        /// <summary>
        /// Consulta las opciones del rol
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Consulta(string id)
        {
            RolViewModel model = new RolViewModel();
            model.rol = new Rol();

            model = InicializarRolEdit();

            var objRol = _rolRepository.GetSingle(x => x.IdRol == Convert.ToInt32(id));

            model.rol.IdRol = objRol.IdRol;
            model.rol.FlgEstado = objRol.FlgEstado;
            model.rol.FlgSede = objRol.FlgSede;
            model.rol.CodRol = objRol.CodRol;
            model.rol.DscRol = objRol.DscRol;

            model.Accion = Accion.Consultar;

            return View("Edit", model);
        }


    }
}
