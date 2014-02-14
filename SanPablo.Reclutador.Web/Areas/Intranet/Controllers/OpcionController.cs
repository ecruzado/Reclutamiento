
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


    public class OpcionController : BaseController
    {
        

        private IRolRepository _rolRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IRolOpcionRepository _rolOpcionRepository;
        private IOpcionRepository _opcionRepository;

        public OpcionController(IRolRepository rolRepository, IDetalleGeneralRepository detalleGeneralRepository,
                             IRolOpcionRepository rolOpcionRepository, IOpcionRepository opcionRepository)
        {
            _rolRepository = rolRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _rolOpcionRepository = rolOpcionRepository;
            _opcionRepository = opcionRepository;
        }

        /// <summary>
        /// Inicializa el Controlador
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Inicializa la carga del popup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult PopupOpcion(int id)
        {

            OpcionViewModel objOpcion = new OpcionViewModel();
            
            objOpcion = InicializarOpcionIndex();
            objOpcion.IdRoll = id;

            return View("PopupOpcion", objOpcion);

        }

        /// <summary>
        /// Inicializa la lista de valores
        /// </summary>
        /// <returns></returns>
        public OpcionViewModel InicializarOpcionIndex()
        {
            var objOpcion = new OpcionViewModel();
            objOpcion.opcion = new Opcion();

            objOpcion.TipoEstado =
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoMant));
            objOpcion.TipoEstado.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return objOpcion;
        }


        /// <summary>
        /// obtiene los datos de la lista de opciones
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaPopupOpcion(GridTable grid)
        {
            try
            {
                // int idCriterio = Convert.ToInt32(grid.rules[0].data);
                DetachedCriteria where = null;
                where = DetachedCriteria.For<Opcion>();

                if ((!"".Equals(grid.rules[0].data) && grid.rules[0].data!=null) ||
                    (!"".Equals(grid.rules[1].data) && grid.rules[1].data!=null) ||
                    (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0")
                   )
                {
                   

                    if (!"".Equals(grid.rules[0].data) && grid.rules[0].data!=null)
                    {
                        where.Add(Expression.Like("DSCOPCION", '%'+grid.rules[0].data+'%'));
                    }
                    if (!"".Equals(grid.rules[1].data) && grid.rules[1].data!=null)
                    {
                        where.Add(Expression.Like("DESCRIPCION", '%'+grid.rules[1].data+'%'));
                    }
                    if (!"".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0")
                    {
                        where.Add(Expression.Eq("FLGHABILITADO", grid.rules[2].data));
                    }

                }

                if (where != null)
                {
                    where.Add(Expression.IsNotNull("IDOPCION"));
                }

                var generic = Listar(_opcionRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IDITEM.ToString(),
                    cell = new string[]
                            {
                                "1",
                                item.FLGHABILITADO==null?"":item.FLGHABILITADO,
                                item.IDOPCIONPADRE==null?"":item.IDOPCIONPADRE.ToString(),
                                item.IDOPCION==null?"":item.IDOPCION.ToString(),
                                item.DSCOPCION==null?"":item.DSCOPCION.ToString(),
                                item.DESCRIPCION==null?"":item.DESCRIPCION.ToString()
                                
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
        /// obtitne las opciones seleccionadas y las asocia al rol
        /// </summary>
        /// <param name="selc"></param>
        /// <param name="codExamen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetListaOpcion(List<int> selc, string codRol)
        {
            DateTime Hoy = DateTime.Today;
            RolOpcion objRolOpcion;
            JsonMessage objJson = new JsonMessage();
            int idOpcion=0;
            int codigo = 0;
            int codItem = 0;

            if (codRol != null)
            {
                codigo = Convert.ToInt32(codRol);
            }
            else
            {
                codigo = 0;
            }

            if (selc != null && selc.Count > 0)
            {
                for (int i = 0; i < selc.Count; i++)
                {

                    //objRolOpcion = new RolOpcion();
                    //objRolOpcion.Opcion = new Opcion();
                    codItem = selc[i] == null ? 0 : selc[i];

                    var objOpcion = _opcionRepository.GetSingle(x => x.IDITEM == codItem);
                    idOpcion = (objOpcion.IDOPCION == null ? 0 : objOpcion.IDOPCION);

                   
                    var rolOpcion = _rolOpcionRepository.GetBy(x => x.IDOPCION == idOpcion
                                                                 && x.IDROL == codigo);

                    if (rolOpcion != null && rolOpcion.Count > 0)
                    {
                        continue;
                    }
                    else
                    {
                        objRolOpcion = new RolOpcion();
                        objRolOpcion.IDROL = codigo;
                        objRolOpcion.IDOPCION = idOpcion;
                        objRolOpcion.USRCREACION = "Usuario Session";
                        objRolOpcion.FECCREACION = Hoy;
                        objRolOpcion.USRMODIFICACION = "Usuario Session";
                        objRolOpcion.FECMODIFICACION = Hoy;

                        _rolOpcionRepository.Add(objRolOpcion);

                        objJson.Resultado = true;

                    }

                }
            }

            return Json(objJson); ;
        }

       

        

    }
}
