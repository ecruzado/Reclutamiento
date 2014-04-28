namespace SanPablo.Reclutador.Web.Controllers
{
    
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using SanPablo.Reclutador.Web.Models;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Entity;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using NHibernate.Criterion;

    public class ExperienciaPostulanteController : BaseController
    {
        private IExperienciaPostulanteRepository _experienciaPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IPostulanteRepository _postulanteRepository;
        private IUsuarioRepository _usuarioRepository;
      
        public ExperienciaPostulanteController(IExperienciaPostulanteRepository experienciaPostulanteRepository, 
                                               IDetalleGeneralRepository detalleGeneralRepository,
                                               IPostulanteRepository postulanteRepository,
                                               IUsuarioRepository usuarioRepository
      
                                                )
        {
            _experienciaPostulanteRepository = experienciaPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _postulanteRepository = postulanteRepository;
            _usuarioRepository = usuarioRepository;
            
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Index()
        {
            var experienciaViewModel = InicializarExperiencia();
            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            experienciaViewModel.Experiencia.Postulante = postulante;
            return View(experienciaViewModel);
        }

        [HttpPost]
        public virtual JsonResult Listar(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ExperienciaPostulante>();
                where.Add(Expression.Eq("Postulante.IdePostulante", IdePostulante));
                
                var generic = Listar(_experienciaPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaPostulante.ToString(),
                        cell = new string[]
                            {
                                item.NombreEmpresa==null?"": item.NombreEmpresa.ToUpper(),
                                item.DescripcionCargoTrabajo==null?"": item.DescripcionCargoTrabajo.ToUpper(),
                                //String.Format("{0:dd/MM/yyyy}", item.FechaTrabajoInicio),
                                item.FechaInicio==null?"": item.FechaInicio,
                                item.IndicadorActualmenteTrabajo== "S"?"Actualmente": item.FechaFin,
                                //item.FechaTrabajoFin.ToString(),
                                item.IndicadorActualmenteTrabajo,
                                item.TiempoDeServicio == null?"":item.TiempoDeServicio,
                                item.DescripcionMotivoCese == null?"":item.DescripcionMotivoCese,
                                item.NombreReferente ==null?"": item.NombreReferente.ToUpper(),
                                item.NumeroMovilReferencia == null?"":item.NumeroMovilReferencia.ToString(),
                                item.DescripcionCargoReferente == null?"":item.DescripcionCargoReferente,
                                item.NumeroFijoInstitucionReferente == null?"":item.NumeroFijoInstitucionReferente.ToString(),
                                item.NumeroAnexoInstitucionReferente == null?"":item.NumeroAnexoInstitucionReferente.ToString()
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

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ViewResult Edit(string id)
        {
            var experienciaGeneralViewModel = InicializarExperiencia();
            if (id == "0")
            {
                return View(experienciaGeneralViewModel);
            }
            else
            {
                var experienciaResultado = new ExperienciaPostulante();
                experienciaResultado = _experienciaPostulanteRepository.GetSingle(x => x.IdeExperienciaPostulante == Convert.ToInt32(id));
                experienciaGeneralViewModel.Experiencia = experienciaResultado;
                return View(experienciaGeneralViewModel);
            }
        }

        [HttpPost]
        public JsonResult Edit([Bind(Prefix = "Experiencia")]ExperienciaPostulante experienciaPostulante)
        {
            var objetoUsuario = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];
            string usuarioActual = objetoUsuario.CodUsuario.Length <= 15 ? objetoUsuario.CodUsuario : objetoUsuario.CodUsuario.Substring(0, 15);

            if (!ModelState.IsValid)
            {
                return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
            }
            if (experienciaPostulante.IdeExperienciaPostulante == 0)
            {
                if (IdePostulante != 0)
                {
                    
                    experienciaPostulante.EstadoActivo = IndicadorActivo.Activo;
                    experienciaPostulante.FechaCreacion = FechaCreacion;
                    experienciaPostulante.UsuarioCreacion = usuarioActual;
                    var postulante = new Postulante();
                    postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                    postulante.agregarExperiencia(experienciaPostulante);
                    _experienciaPostulanteRepository.Add(experienciaPostulante);
                }
                else
                {
                    return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                var experienciaEdit = _experienciaPostulanteRepository.GetSingle(x => x.IdeExperienciaPostulante == experienciaPostulante.IdeExperienciaPostulante);
                experienciaEdit.TipoMotivoCese = experienciaPostulante.TipoMotivoCese;
                experienciaEdit.TipoCargoTrabajoReferente = experienciaPostulante.TipoCargoTrabajoReferente;
                experienciaEdit.TipoCargoTrabajo = experienciaPostulante.TipoCargoTrabajo;
                experienciaEdit.TiempoDeServicio = experienciaPostulante.TiempoDeServicio;
                experienciaEdit.NumeroMovilReferencia = experienciaPostulante.NumeroMovilReferencia;
                experienciaEdit.NumeroFijoInstitucionReferente = experienciaPostulante.NumeroFijoInstitucionReferente;
                experienciaEdit.NumeroAnexoInstitucionReferente = experienciaPostulante.NumeroAnexoInstitucionReferente;
                experienciaEdit.NombreReferente = experienciaPostulante.NombreReferente;
                experienciaEdit.NombreEmpresa = experienciaPostulante.NombreEmpresa;
                experienciaEdit.NombreCargoTrabajo = experienciaPostulante.NombreCargoTrabajo;
                experienciaEdit.IndicadorActualmenteTrabajo = experienciaPostulante.IndicadorActualmenteTrabajo;
                experienciaEdit.FechaTrabajoInicio = experienciaPostulante.FechaTrabajoInicio;
                experienciaEdit.FechaTrabajoFin = experienciaPostulante.FechaTrabajoFin;
                experienciaEdit.CorreoReferente = experienciaPostulante.CorreoReferente;
                experienciaEdit.FuncionesDesempenadas = experienciaPostulante.FuncionesDesempenadas;
                experienciaEdit.FechaModificacion = FechaModificacion;
                experienciaEdit.UsuarioModificacion = usuarioActual;
               

                _experienciaPostulanteRepository.Update(experienciaEdit);
            }
            return Json(new { msj = true }, JsonRequestBehavior.DenyGet);
        }

        public ExperienciaPostulanteGeneralViewModel InicializarExperiencia()
        {
            var experienciaPostulanteGeneralViewModel = new ExperienciaPostulanteGeneralViewModel();
            experienciaPostulanteGeneralViewModel.Experiencia = new ExperienciaPostulante();

            experienciaPostulanteGeneralViewModel.porcentaje = Convert.ToInt32(Session["Progreso"]);

            experienciaPostulanteGeneralViewModel.TipoCargos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCargo));
            experienciaPostulanteGeneralViewModel.TipoCargos.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTROS" });
            experienciaPostulanteGeneralViewModel.TipoCargos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            experienciaPostulanteGeneralViewModel.TipoMotivosCese = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoMotivoCese));
            experienciaPostulanteGeneralViewModel.TipoMotivosCese.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            experienciaPostulanteGeneralViewModel.TipoCargosReferente = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCargo));
            experienciaPostulanteGeneralViewModel.TipoCargosReferente.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            return experienciaPostulanteGeneralViewModel;
        }

        [HttpPost]
        public ActionResult eliminarExperiencia(int ideExperiencia)
        {
            
            ActionResult result = null;

            var experienciaEliminar = new ExperienciaPostulante();
            experienciaEliminar = _experienciaPostulanteRepository.GetSingle(x => x.IdeExperienciaPostulante == ideExperiencia);
            _experienciaPostulanteRepository.Remove(experienciaEliminar);
            return result;
        }

        [HttpPost]
        public ActionResult calcularTiempoServicio(string inicio, string fin)
        {
            DateTime fechaInicio = Convert.ToDateTime(inicio);
            DateTime fechaFin = Convert.ToDateTime(fin);

            ActionResult result = null;

            if (fechaFin.Year == 1000)
            {
                fechaFin = DateTime.Now;
            }

            var days = fechaFin - fechaInicio;

            int anhos = days.Days / 365;
            int resto = days.Days % 365;
            int meses = resto / 30;

            string tiempoServicio = anhos.ToString() + " AÑO(S) Y " + meses.ToString() + " MES(ES)";
            return result = Json(tiempoServicio);
        }


        /// <summary>
        /// valida experiencia del postulante
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult validaExperiencia()
        {
            JsonMessage objMensaje = new JsonMessage();
            int idUsuario = 0;

            idUsuario = (Session[ConstanteSesion.Usuario] == null ? 0 : (Convert.ToInt32(Session[ConstanteSesion.Usuario])));

            var ObjUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == idUsuario
                                                          && x.TipUsuario == TipUsuario.Extranet
                                                          && x.FlgEstado == IndicadorActivo.Activo);

            if (ObjUsuario != null)
            {
                List<ExperienciaPostulante> listaExperiencia = (List<ExperienciaPostulante>)_experienciaPostulanteRepository.GetBy(x => x.Postulante.IdePostulante == ObjUsuario.IdePostulante);

                if (listaExperiencia != null && listaExperiencia.Count > 0)
                {
                    objMensaje.Resultado = true;
                }
                else
                {
                    objMensaje.Resultado = false;
                    objMensaje.Mensaje = "Registre sus experiencias";
                }
            }

            return Json(objMensaje);
        }


    }
}
