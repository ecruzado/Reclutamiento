namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using NHibernate.Criterion;
    public class EstudioPostulanteController : BaseController
    {
        private IEstudioPostulanteRepository _estudioPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IPostulanteRepository _postulanteRepository;
        private IUsuarioRepository _usuarioRepository;
               
        public EstudioPostulanteController(IEstudioPostulanteRepository estudioPostulanteRepository, 
                                           IDetalleGeneralRepository detalleGeneralRepository,
                                           IPostulanteRepository postulanteRepository,
                                            IUsuarioRepository usuarioRepository
                                            )
        {
            _estudioPostulanteRepository = estudioPostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _postulanteRepository = postulanteRepository;
            _usuarioRepository = usuarioRepository;
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Index()
        {
            var estudioGeneralViewModel = InicializarEstudio();
            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            estudioGeneralViewModel.Estudio.Postulante = postulante;
            return View(estudioGeneralViewModel);
        }
   
        [HttpPost]
        public virtual JsonResult Listar(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<EstudioPostulante>();
                where.Add(Expression.Eq("Postulante.IdePostulante",IdePostulante));

                var generic = Listar(_estudioPostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeEstudiosPostulante.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionEducacion,
                                item.DescripcionNombreInstitucion.ToUpper(),
                                item.DescripcionArea,
                                item.DescripcionNivelAlcanzado,
                                //String.Format("{0:dd/MM/yyyy}", item.FechaEstudioInicio), 
                                //String.Format("{0:dd/MM/yyyy}", item.FechaEstudioFin)
                                item.FechaInicio,
                                item.IndicadorActualmenteEstudiando == "S"?"Actualmente":item.FechaFin 
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError("ERROR: "+ex.Message);
            }
        }
        public string datosDetalle(string codigo, int tipoDato)
        {
            var general = _detalleGeneralRepository.GetBy(x => x.IdeGeneral == tipoDato);
            var dato = general.Single(x => x.Valor == codigo);
            return dato.Descripcion;
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ViewResult Edit(string id)
        {
            var estudioGeneralViewModel = InicializarEstudio();
            if (id == "0")
            {
               return View(estudioGeneralViewModel);
            }
            else
            {
               var estudioResultado = new EstudioPostulante();
               estudioResultado = _estudioPostulanteRepository.GetSingle(x => x.IdeEstudiosPostulante == Convert.ToInt32(id));
               estudioGeneralViewModel.Estudio = estudioResultado;
               estudioGeneralViewModel = actualizarDatos(estudioGeneralViewModel, estudioResultado);
               return View(estudioGeneralViewModel);
           }
           
        }


        [HttpPost]
        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Edit([Bind(Prefix = "Estudio")]EstudioPostulante estudioPostulante)
        {
            var objetoUsuario = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];
            string usuarioActual = objetoUsuario.CodUsuario.Length <= 15 ? objetoUsuario.CodUsuario : objetoUsuario.CodUsuario.Substring(0, 15);

            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    objJsonMessage.Mensaje = "Verifique que todos los datos obligatorios esten registrados";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
                if (estudioPostulante.IdeEstudiosPostulante == 0)
                {
                    if (IdePostulante != 0)
                    {
                        estudioPostulante.EstadoActivo = IndicadorActivo.Activo;
                        estudioPostulante.FechaCreacion = FechaCreacion;
                        estudioPostulante.UsuarioCreacion = usuarioActual;
                        var postulante = new Postulante();
                        postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                        postulante.agregarEstudio(estudioPostulante);
                        _estudioPostulanteRepository.Add(estudioPostulante);
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "Verifique su session";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    var estudioEdit = _estudioPostulanteRepository.GetSingle(x => x.IdeEstudiosPostulante == estudioPostulante.IdeEstudiosPostulante);
                    estudioEdit.TipTipoInstitucion = estudioPostulante.TipTipoInstitucion;
                    estudioEdit.TipoNombreInstitucion = estudioPostulante.TipoNombreInstitucion;
                    estudioEdit.TipoNivelAlcanzado = estudioPostulante.TipoNivelAlcanzado;
                    estudioEdit.TipoEducacion = estudioPostulante.TipoEducacion;
                    estudioEdit.TipoArea = estudioPostulante.TipoArea;
                    estudioEdit.NombreInstitucion = estudioPostulante.NombreInstitucion;
                    estudioEdit.IndicadorActualmenteEstudiando = estudioPostulante.IndicadorActualmenteEstudiando;
                    estudioEdit.FechaInicio = estudioPostulante.FechaInicio;
                    estudioEdit.FechaFin = estudioPostulante.FechaFin;
                    estudioEdit.FechaModificacion = FechaModificacion;
                    estudioEdit.UsuarioModificacion = usuarioActual;

                    _estudioPostulanteRepository.Update(estudioEdit);
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
            
        }

        public EstudioPostulanteGeneralViewModel InicializarEstudio()
        {
            var estudioPostulanteGeneralViewModel = new EstudioPostulanteGeneralViewModel();
            estudioPostulanteGeneralViewModel.Estudio = new EstudioPostulante();

            estudioPostulanteGeneralViewModel.porcentaje = Convert.ToInt32(Session["Progreso"]);

            estudioPostulanteGeneralViewModel.TipoTipoInstituciones = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoInstitucion));
            estudioPostulanteGeneralViewModel.TipoTipoInstituciones.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            estudioPostulanteGeneralViewModel.TipoNombreInstituciones = new List<DetalleGeneral>();
            estudioPostulanteGeneralViewModel.TipoNombreInstituciones.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });
           
            estudioPostulanteGeneralViewModel.AreasEstudio = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoArea).OrderBy(x=>x.Descripcion));
            estudioPostulanteGeneralViewModel.AreasEstudio.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            estudioPostulanteGeneralViewModel.TiposEducacion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEducacion));
            estudioPostulanteGeneralViewModel.TiposEducacion.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            estudioPostulanteGeneralViewModel.NivelesAlcanzados = new List<DetalleGeneral>();
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });

            return estudioPostulanteGeneralViewModel;
        }
        

        [HttpPost]
        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult eliminarEstudio(int ideEstudio)
        {
            ActionResult result = null;

            var estudioEliminar = new EstudioPostulante();
            estudioEliminar = _estudioPostulanteRepository.GetSingle(x => x.IdeEstudiosPostulante == ideEstudio);
            _estudioPostulanteRepository.Remove(estudioEliminar);
            return result;
        }


        #region METODOS

        [HttpPost]
        public ActionResult listarNombreInstitucion(string tipoInstituto)
        {
            ActionResult result = null;
            var listaResultado = new List<DetalleGeneral>();

            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoInstitucion,tipoInstituto).OrderBy(x=>x.Descripcion));
            listaResultado.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTROS" });
            result = Json(listaResultado);
            return result;
        }
        

        [HttpPost]
        public ActionResult listarNivelAlcanzado(string tipoEducacion)
        {
            ActionResult result = null;
            var listaResultado = new List<DetalleGeneral>();

            listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoEducacion, tipoEducacion).OrderBy(x=>x.Descripcion));
            listaResultado.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTROS" });

            result = Json(listaResultado);
            return result;
        }


        public EstudioPostulanteGeneralViewModel actualizarDatos(EstudioPostulanteGeneralViewModel estudioPostulanteGeneralViewModel, EstudioPostulante estudioPostulante)
        {
             if (estudioPostulante != null)
            {
                var listaResultado = new List<DetalleGeneral>();
                var listaResultadoNiveles = new List<DetalleGeneral>();
                listaResultado = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoInstitucion, estudioPostulante.TipTipoInstitucion));
                listaResultado.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTROS" });
                estudioPostulanteGeneralViewModel.TipoNombreInstituciones = listaResultado;

                listaResultadoNiveles = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTableReference(TipoTabla.TipoEducacion,estudioPostulante.TipoEducacion));
                //listaResultadoNiveles.Add(new DetalleGeneral { Valor = "XX", Descripcion = "OTRO" });
                estudioPostulanteGeneralViewModel.NivelesAlcanzados = listaResultadoNiveles;
            }
             return estudioPostulanteGeneralViewModel;
        }

        //[ValidarSesion]
        [HttpPost]
        public ActionResult validaEstudios()
        {
            JsonMessage objMensaje= new JsonMessage();
            int idUsuario=0;

            idUsuario = (Session[ConstanteSesion.Usuario] == null ? 0 : (Convert.ToInt32(Session[ConstanteSesion.Usuario])));

            var ObjUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == idUsuario 
                                                          && x.TipUsuario == TipUsuario.Extranet 
                                                          && x.FlgEstado == IndicadorActivo.Activo);

            if (ObjUsuario!=null)
            {
               List<EstudioPostulante> listaPostulantes = (List<EstudioPostulante>)_estudioPostulanteRepository.GetBy(x => x.Postulante.IdePostulante==ObjUsuario.IdePostulante);

               if (listaPostulantes!=null && listaPostulantes.Count>0)
               {
                   objMensaje.Resultado = true;
               }
               else
               {
                   objMensaje.Resultado = false;
                   objMensaje.Mensaje = "Registre sus estudios";
               }
            }

            return Json(objMensaje);
        }




        #endregion
    }
}
