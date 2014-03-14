﻿namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;

    [Authorize]
    public class MantenimientoCargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IUsuarioRepository _usuarioRepository;
        private IListaSolicitudNuevoCargoVistaRepository _listaSolicitudRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IAreaRepository _areaRepository;

        public MantenimientoCargoController(ICargoRepository cargoRepository,
                                            IDetalleGeneralRepository detalleGeneralRepository,
                                            IUsuarioRepository usuarioRepository,
                                            IListaSolicitudNuevoCargoVistaRepository listaSolicitudRepository,
                                            IDependenciaRepository dependenciaRepository,
                                            IDepartamentoRepository departamentoRepository,
                                            IAreaRepository areaRepository)
        {
            _cargoRepository = cargoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            _areaRepository = areaRepository;
            _usuarioRepository = usuarioRepository;
            _listaSolicitudRepository = listaSolicitudRepository;
        }

        /// <summary>
        /// inicio del mantenimiento de cargo
        /// </summary>
        /// <param name="id" : Identificador de Cargo a dar mantenimiento></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Index(string id)
        {
            int ideCargo = Convert.ToInt32(id);
            Cargo cargo = new Cargo();
            var mantenimientoViewModel = inicializarCargo();
            mantenimientoViewModel.Accion = Accion.Consultar;
            try
            {
                if (ideCargo != 0)
                {
                    cargo = _cargoRepository.GetSingle(x => x.IdeCargo == ideCargo);
                    actualizarDatosSession(cargo);
                    ModoAccion = Accion.Editar;
                    mantenimientoViewModel.Accion = Accion.Editar;
                    mantenimientoViewModel.Cargo = cargo;
                }
                if (CargoPerfil != null)
                {
                    cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                    mantenimientoViewModel.Accion = ModoAccion;
                    mantenimientoViewModel.Cargo = cargo;
                    actualizarDatosCargo(mantenimientoViewModel, cargo);
                }

                return View("Index", mantenimientoViewModel);
            }
            catch (Exception)
            {
                return View("Index");
            }

        }

        /// <summary>
        /// Inicializar en modo Consulta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Consultar(string id)
        {
            
            int ideCargo = Convert.ToInt32(id);
            var mantenimientoViewModel = inicializarCargo();
            try
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == ideCargo);
                actualizarDatosSession(cargo);
                mantenimientoViewModel.Cargo = cargo;
                actualizarDatosCargo(mantenimientoViewModel, cargo);

                mantenimientoViewModel.Accion = Accion.Consultar;
                ModoAccion = Accion.Consultar;

                return View("Index", mantenimientoViewModel);
            }
            catch (Exception)
            {
                return View("Index");
            }

        }


        public void actualizarDatosSession(Cargo cargo)
        {
            var usuario = Session[ConstanteSesion.UsuarioDes].ToString();
            
            CargoPerfil = new DatosCargo();
            CargoPerfil.IdeCargo = cargo.IdeCargo;
            CargoPerfil.CodigoCargo = cargo.CodigoCargo;

            var datosArea = _areaRepository.obtenerDatosArea(cargo.IdeArea);
            CargoPerfil.Area = datosArea[1];
            CargoPerfil.Departamento = datosArea[3];
            CargoPerfil.Dependencia = datosArea[5];
            
        }
        /// <summary>
        /// Gurada los datos de la pestaña puesto
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index([Bind(Prefix = "Cargo")]Cargo cargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var cargoViewModel = inicializarCargo();
            try
            {
                CargoValidator validation = new CargoValidator();
                ValidationResult result = validation.Validate(cargo, "ObjetivoCargo", "FuncionCargo");
                if (!result.IsValid)
                {
                    cargoViewModel.Cargo = cargo;
                    return View(cargoViewModel);
                }

                cargoEditar.UsuarioModificacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                cargoEditar.FechaModificacion = FechaCreacion;
                cargoEditar.ObjetivoCargo = cargo.ObjetivoCargo;
                cargoEditar.FuncionCargo = cargo.FuncionCargo;
                _cargoRepository.Update(cargoEditar);

                return RedirectToAction("../MantenimientoCargo/General");
            }
            catch (Exception ex)
            {
                cargoViewModel.Cargo = cargo;
                return View(cargoViewModel);
            }
        }

        /// <summary>
        /// Inicialización de Mantenimiento ViewModel
        /// </summary>
        /// <returns></returns>
        public MantenimientoCargoViewModel inicializarCargo()
        {
            var cargoViewModel = new MantenimientoCargoViewModel();
            cargoViewModel.Cargo = new Cargo();

            cargoViewModel.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.Sexo));
            cargoViewModel.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            cargoViewModel.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            cargoViewModel.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            cargoViewModel.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            cargoViewModel.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            cargoViewModel.Accion = Convert.ToString(Session[ConstanteSesion.Modo]);

            return cargoViewModel;
        }

        /// <summary>
        /// Iniciar pestaña General de Mantenimiento de Cargo
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult General()
        {
            var perfilViewModel = inicializarCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(perfilViewModel, cargo);
                perfilViewModel.Cargo = cargo;
            }
            return View(perfilViewModel);
        }

        /// <summary>
        /// Guradar los cambios de la pestaña generales
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult General([Bind(Prefix = "Cargo")]Cargo cargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var cargoViewModel = inicializarCargo();
            
            try
            {
                var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
                CargoValidator validation = new CargoValidator();
                ValidationResult resul = validation.Validate(cargo, "PuntajePostulanteInterno", "EdadInicio", "EdadFin",
                                                             "PuntajeEdad", "Sexo", "PuntajeSexo", "TipoRequerimiento", "TipoRangoSalarial", "PuntajeSalario");

                if (!resul.IsValid)
                {
                    cargoViewModel.Cargo = cargo;
                    return View(cargoViewModel);
                }
                cargoEditar.UsuarioModificacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                cargoEditar.FechaModificacion = FechaCreacion;
                cargoEditar.PuntajeTotalPostulanteInterno = cargo.PuntajeTotalPostulanteInterno;
                cargoEditar.EdadInicio = cargo.EdadInicio;
                cargoEditar.EdadFin = cargo.EdadFin;
                cargoEditar.PuntajeEdad = cargo.PuntajeEdad;
                cargoEditar.Sexo = cargo.Sexo;
                cargoEditar.PuntajeSexo = cargo.PuntajeSexo;
                cargoEditar.TipoRequerimiento = cargo.TipoRequerimiento;
                cargoEditar.TipoRangoSalarial = cargo.TipoRangoSalarial;
                cargoEditar.IndicadorEdadRanking = cargo.IndicadorEdadRanking;
                cargoEditar.IndicadorSalarioRanking = cargo.IndicadorSalarioRanking;
                cargoEditar.IndicadorSexoRanking = cargo.IndicadorSexoRanking;
                cargoEditar.PuntajeSalario = cargo.PuntajeSalario;
                _cargoRepository.Update(cargoEditar);

                return RedirectToAction("../MantenimientoCargo/Estudio");
            }
            catch (Exception ex)
            {
                cargoViewModel.Cargo = cargo;
                return View(cargoViewModel);
            }

        }

        /// <summary>
        /// Inicicalizacion de pestaña Estudio
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Estudio()
        {
            var estudioCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(estudioCargoViewModel, cargo);
            }
            return View("Estudio", estudioCargoViewModel);
        }

        /// <summary>
        /// Inicializacion de datos de Experiencia
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Experiencia()
        {

            var experienciaCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(experienciaCargoViewModel, cargo);
            }
            return View("Experiencia",experienciaCargoViewModel);
        }

        /// <summary>
        /// Inicialializacion de pestaña Conocimientos
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Conocimientos()
        {
            var conocimientosCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(conocimientosCargoViewModel, cargo);
            }
            return View(conocimientosCargoViewModel);
        }

        /// <summary>
        /// Inicializacion Pestaña Discapacidad
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Discapacidad()
        {
            var discapacidadCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(discapacidadCargoViewModel, cargo);
            }
            return View(discapacidadCargoViewModel);
        }

        /// <summary>
        /// Inicializar Configuracion de Perfil
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult ConfiguracionPerfil()
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var discapacidadCargoViewModel = inicializarDatosConfig(IdeCargo);
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(discapacidadCargoViewModel, cargo);
            }
            return View(discapacidadCargoViewModel);
        }

        /// <summary>
        /// inicicalizacion basica 
        /// </summary>
        /// <returns></returns>
        public MantenimientoCargoViewModel inicializarDatosCargo()
        {
            var mantenimientoViewModel = new MantenimientoCargoViewModel();
            mantenimientoViewModel.Cargo = new Cargo();

            mantenimientoViewModel.Accion = Convert.ToString(Session[ConstanteSesion.Modo]);
            return mantenimientoViewModel;
        }

        /// <summary>
        /// Datos de Configuracion (Puntajes)
        /// </summary>
        /// <param name="IdeCargo"></param>
        /// <returns></returns>
        public MantenimientoCargoViewModel inicializarDatosConfig(int IdeCargo)
        {
            var cargoViewModel = new MantenimientoCargoViewModel();
            var cargoActual = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            cargoViewModel.TotalMaximo = 0;
            cargoViewModel.Cargo = cargoActual;
            return cargoViewModel;
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult calcularPuntajeTotal()
        {
            var cargoActual = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
            JsonMessage objJsonMessage = new JsonMessage();
            
            try
            {
                int puntajeTotal = Convert.ToInt32(cargoActual.PuntajeTotalCentroEstudio) + Convert.ToInt32(cargoActual.PuntajeTotalConocimientoGeneral) + Convert.ToInt32(cargoActual.PuntajeTotalDiscapacidad) +
                                   Convert.ToInt32(cargoActual.PuntajeTotalEdad) + Convert.ToInt32(cargoActual.PuntajeTotalExamen) + Convert.ToInt32(cargoActual.PuntajeTotalExperiencia) + Convert.ToInt32(cargoActual.PuntajeTotalHorario) +
                                   Convert.ToInt32(cargoActual.PuntajeTotalIdioma) + Convert.ToInt32(cargoActual.PuntajeTotalNivelEstudio) + Convert.ToInt32(cargoActual.PuntajeTotalOfimatica) + Convert.ToInt32(cargoActual.PuntajeTotalPostulanteInterno) +
                                   Convert.ToInt32(cargoActual.PuntajeTotalSalario) + Convert.ToInt32(cargoActual.PuntajeTotalSexo) + Convert.ToInt32(cargoActual.PuntajeTotalUbigeo);
                
                objJsonMessage.IdDato = puntajeTotal;
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "Error" + ex;
                objJsonMessage.IdDato = 0;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }

        [ValidarSesion]
        [HttpPost]
        public ActionResult ConfiguracionPerfil([Bind(Prefix = "Cargo")]Cargo cargo)
        {
            int IdeCargo = CargoPerfil.IdeCargo;
            var cargoEditar = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
            var cargoViewModel = inicializarDatosCargo();

            try
            {
                CargoValidator validation = new CargoValidator();
                ValidationResult result = validation.Validate(cargo, "PuntajeMinimoPostulanteInterno", "PuntajeMinimoEdad", "PuntajeMinimoSexo", "PuntajeMinimoSalario",
                                                              "PuntajeMinimoNivelEstudio", "PuntajeMinimoCentroEstudio", "PuntajeMinimoExperiencia", "PuntajeMinimoOfimatica", "PuntajeMinimoIdioma", "PuntajeMinimoConocimientoGeneral",
                                                              "PuntajeMinimoDiscapacidad", "PuntajeMinimoHorario", "PuntajeMinimoUbigeo", "PuntajeMinimoExamen");

                cargoEditar.UsuarioModificacion = Convert.ToString(Session[ConstanteSesion.UsuarioDes]);
                cargoEditar.FechaModificacion = FechaCreacion;
                cargoEditar.PuntajeMinimoPostulanteInterno = cargo.PuntajeMinimoPostulanteInterno;
                cargoEditar.PuntajeMinimoEdad = cargo.PuntajeMinimoEdad;
                cargoEditar.PuntajeMinimoSexo = cargo.PuntajeMinimoSexo;
                cargoEditar.PuntajeMinimoSalario = cargo.PuntajeMinimoSalario;
                cargoEditar.PuntajeMinimoNivelEstudio = cargo.PuntajeMinimoNivelEstudio;
                cargoEditar.PuntajeMinimoCentroEstudio = cargo.PuntajeMinimoCentroEstudio;
                cargoEditar.PuntajeMinimoExperiencia = cargo.PuntajeMinimoExperiencia;
                cargoEditar.PuntajeMinimoOfimatica = cargo.PuntajeMinimoOfimatica;
                cargoEditar.PuntajeMinimoIdioma = cargo.PuntajeMinimoIdioma;
                cargoEditar.PuntajeMinimoConocimientoGeneral = cargo.PuntajeMinimoConocimientoGeneral;
                cargoEditar.PuntajeMinimoDiscapacidad = cargo.PuntajeMinimoDiscapacidad;
                cargoEditar.PuntajeMinimoHorario = cargo.PuntajeMinimoHorario;
                cargoEditar.PuntajeMinimoUbigeo = cargo.PuntajeMinimoUbigeo;
                cargoEditar.PuntajeMinimoExamen = cargo.PuntajeMinimoExamen;
                cargoEditar.CantidadPreseleccionados = cargo.CantidadPreseleccionados;

                if (!result.IsValid)
                {
                    cargoViewModel.Cargo = cargoEditar;
                    actualizarDatosCargo(cargoViewModel, cargo);
                    return View(cargoViewModel);
                }

                _cargoRepository.Update(cargoEditar);
                cargoViewModel.Cargo = cargoEditar;
                actualizarDatosCargo(cargoViewModel, cargo);

                return View(cargoViewModel);
            }
            catch (Exception ex)
            {
                cargoViewModel.Cargo = cargo;
                return View(cargoViewModel);
            }

        }

        /// <summary>
        /// Inicializacion de la pestaña Evaluaciones
        /// </summary>
        /// <returns></returns>
        public ActionResult Evaluacion()
        {
            var evaluacionCargoViewModel = inicializarDatosCargo();
            if (CargoPerfil != null)
            {
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == CargoPerfil.IdeCargo);
                actualizarDatosCargo(evaluacionCargoViewModel, cargo);
            }
            return View(evaluacionCargoViewModel);
        }

        /// <summary>
        /// Actualizar los datos del area y cargo
        /// </summary>
        /// <param name="perfilViewModel"></param>
        /// <param name="cargo"></param>
        public void actualizarDatosCargo(MantenimientoCargoViewModel perfilViewModel, Cargo cargo)
        {
            perfilViewModel.Cargo.CodigoCargo = CargoPerfil.CodigoCargo;
            perfilViewModel.Cargo.NombreCargo = CargoPerfil.NombreCargo;
            perfilViewModel.Cargo.DescripcionCargo = CargoPerfil.DescripcionCargo;
            perfilViewModel.Cargo.IdeCargo = CargoPerfil.IdeCargo;
            perfilViewModel.Area = CargoPerfil.Area;
            perfilViewModel.Dependencia = CargoPerfil.Dependencia;
            perfilViewModel.Departamento = CargoPerfil.Departamento;

            if (cargo.EstadoActivo == IndicadorActivo.Activo)
            { perfilViewModel.EstadoRegistro = "ACTIVO"; }
            else
            { perfilViewModel.EstadoRegistro = "INACTIVO"; }
        }

        /// <summary>
        /// Inicializacion de la pagina inicial
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult ListaCargos()
        {
            var mantenimientoViewModel = inicializarListaMantenimiento();
            return View("ListaCargos", mantenimientoViewModel);
        }

        /// <summary>
        /// Grilla con los cargos que ya finalizaron su proceso de seleccion
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult ListaSolicitudes(GridTable grid)
        {
            try
            {

                DetachedCriteria where = null;
                where = DetachedCriteria.For<ListaSolicitudNuevoCargo>();

                if (
                    (!"Seleccionar".Equals(grid.rules[1].data) && grid.rules[1].data != null && grid.rules[1].data != "0") ||
                    (!"Seleccionar".Equals(grid.rules[2].data) && grid.rules[2].data != null && grid.rules[2].data != "0") ||
                    (!"Seleccionar".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0") ||
                    (!"Seleccionar".Equals(grid.rules[4].data) && grid.rules[4].data != null && grid.rules[4].data != "0") ||
                    (!"Seleccionar".Equals(grid.rules[5].data) && grid.rules[5].data != null && grid.rules[5].data != "0") ||
                    (!"Seleccionar".Equals(grid.rules[6].data) && grid.rules[6].data != null && grid.rules[6].data != "0") ||
                    (!"Seleccionar".Equals(grid.rules[7].data) && grid.rules[7].data != null && grid.rules[7].data != "0") ||
                    (!"Seleccionar".Equals(grid.rules[8].data) && grid.rules[8].data != null && grid.rules[8].data != "0")
                   )
                {

                    if (!"Seleccionar".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data))
                    {
                        where.Add(Expression.Eq("NombreCargo", grid.rules[1].data));
                    }
                    if (!"Seleccionar".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data))
                    {
                        where.Add(Expression.Eq("IdeDependencia", Convert.ToInt32(grid.rules[2].data)));
                    }
                    if (!"Seleccionar".Equals(grid.rules[3].data) && grid.rules[3].data != null && grid.rules[3].data != "0")
                    {
                        where.Add(Expression.Eq("IdeDepartamento", Convert.ToInt32(grid.rules[3].data)));
                    }
                    if (!"Seleccionar".Equals(grid.rules[4].data) && grid.rules[4].data != null && grid.rules[4].data != "0")
                    {
                        where.Add(Expression.Eq("IdeArea", Convert.ToInt32(grid.rules[4].data)));
                    }
                    if (!"Seleccionar".Equals(grid.rules[5].data) && grid.rules[5].data != null && grid.rules[5].data != "0")
                    {
                        where.Add(Expression.Eq("IdeResponsable", Convert.ToInt32(grid.rules[5].data)));
                    }
                    if (!"Seleccionar".Equals(grid.rules[6].data) && grid.rules[6].data != null && grid.rules[6].data != "0")
                    {
                        where.Add(Expression.Eq("TipoEtapa", grid.rules[6].data));
                    }
                    if (!"".Equals(grid.rules[7].data) && grid.rules[7].data != null && grid.rules[7].data != "0")
                    {
                        where.Add(Expression.Eq("EstadoActivo", grid.rules[6].data));
                    }
                    if (!"Seleccionar".Equals(grid.rules[8].data) && grid.rules[7].data != null && grid.rules[7].data != "0")
                    {
                        where.Add(Expression.Ge("FechaCreacion", Convert.ToDateTime(grid.rules[7].data)));
                    }
                    if (!"Seleccionar".Equals(grid.rules[9].data) && grid.rules[8].data != null && grid.rules[8].data != "0")
                    {
                        where.Add(Expression.Le("FechaCreacion", Convert.ToDateTime(grid.rules[8].data)));
                    }

                }

                var generic = Listar(_listaSolicitudRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);
                var i = grid.page * grid.rows;

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeSolicitudNuevoCargo.ToString(),
                    cell = new string[]
                            {
                                "1",
                                item.EstadoActivo==null?"":item.EstadoActivo,
                                item.CodigoCargo==null?"":item.CodigoCargo,
                                item.IdeCargo == null?"":item.IdeCargo.ToString(),
                                item.NombreCargo==null?"":item.NombreCargo,
                                item.IdeDependencia==0?"":item.IdeDependencia.ToString(),
                                item.NombreDependencia==null?"":item.NombreDependencia,
                                item.IdeDepartamento==0?"":item.IdeDepartamento.ToString(),
                                item.NombreDepartamento==null?"":item.NombreDepartamento,
                                item.IdeArea==0?"":item.IdeArea.ToString(),
                                item.NombreArea==null?"":item.NombreArea,
                                item.NumeroPosiciones==0?"0":item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones==0?"0":item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones==0?"0":item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones==0?"0":item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones==0?"0":item.NumeroPosiciones.ToString(),
                                item.NumeroPosiciones==0?"0":item.NumeroPosiciones.ToString(),
                                item.FechaCreacion==null?"":item.FechaCreacion.ToString(),
                                item.FechaCreacion==null?"":item.FechaCreacion.ToString(),
                                item.IdeResponsable ==null?"":item.IdeResponsable.ToString(),
                                item.Responsable==null?"":item.Responsable,
                                
                                item.NombreResponsable==null?"":item.NombreResponsable,
                                item.TipoEtapa==null?"":item.TipoEtapa.ToString(),
                                item.TipoEtapa==null?"":item.TipoEtapa.ToString(),
                                item.Etapa==null?"":item.Etapa,
                                
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
        /// inicializar el model de la lista de cargos
        /// </summary>
        /// <returns></returns>
        public MantenimientoCargoViewModel inicializarListaMantenimiento()
        {
            var model = new MantenimientoCargoViewModel();
            model.Cargo = new Cargo();

            //modificar para filtrar unicamente los cargo cuyo proceso ha concluido
            model.Cargos = new List<Cargo>(_cargoRepository.All());
            model.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            model.Estados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoRegistro));
            model.Estados.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
            model.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            model.Departamentos = new List<Departamento>();
            model.Departamentos.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            model.Areas = new List<Area>();
            model.Areas.Add(new Area { IdeArea = 0, NombreArea = "Seleccionar" });



            return model;
        }
    }
}