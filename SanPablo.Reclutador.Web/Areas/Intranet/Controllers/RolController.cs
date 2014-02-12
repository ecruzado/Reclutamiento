
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
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Edit()
        //{
        //    return View();
        //}

        /// <summary>
        /// Iniaciliza la pag para crear un nuevo Rol
        /// </summary>
        /// <returns></returns>
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
       
        public ActionResult Edit(RolViewModel model)
        {
            JsonMessage objJson = new JsonMessage();
            DateTime hoy = DateTime.Today;

            if (model.rol!=null && model.rol.IdRol != 0)
            {
                var objRrol = _rolRepository.GetSingle(x => x.IdRol == model.rol.IdRol);
                objRrol.FlgSede = model.rol.FlgSede;
                objRrol.FechaModificacion = hoy;
                objRrol.UsuarioModificacion = "Usuario Session";
                objRrol.CodRol = model.rol.CodRol;
                objRrol.DscRol = model.rol.DscRol;
                _rolRepository.Update(objRrol);

                var objModel=InicializarRolEdit();
                objModel.rol = objRrol;
                objModel.Accion = Accion.Editar;

                return View("Edit", objModel);
                //objJson.Resultado = true;
                //objJson.Mensaje = "Se actualizo el rol sastifactoriamente";
                //objJson.IdDato = model.rol.IdRol;

            }
            else
            {
                //nuevo
                model.rol.UsuarioCreacion = "Usuario Sesion";
                model.rol.FechaCreacion = hoy;
                model.rol.FlgEstado = "A";
                _rolRepository.Add(model.rol);
               
                var objModel = InicializarRolEdit();

                objModel.rol = model.rol;
                objModel.Accion = Accion.Editar;
                //objJson.Resultado = true;
                //objJson.Mensaje = "Se registro el rol sastifactoriamente";
                //objJson.IdDato = model.rol.IdRol;
                return View("Edit", objModel);
            }
           
        }

        /// <summary>
        /// 
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
                                "1",
                                "",
                                ""
                                //item.IndicadorActivo,
                                //item.IndicadorActivo,
                                //item.Pregunta,
                                //item.TipoMedicionDes,
                                //item.TipoMedicion,
                                //item.TipoCriterio,
                                //item.TipoCriterioDes,
                                //item.TipoCalificacion,
                                //item.TipoCalificacionDes,
                                //item.TipoModo,
                                //item.TipoModoDes,
                                //item.FechaCreacion == DateTime.MinValue? "": item.FechaCreacion.ToString("dd/MM/yyyy"),
                                //item.UsuarioCreacion,
                                //item.FechaModificacion == DateTime.MinValue? "": item.FechaModificacion.ToString("dd/MM/yyyy"),
                                //item.UsuarioModificacion
                   
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
