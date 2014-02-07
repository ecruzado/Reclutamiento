namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using SanPablo.Reclutador.Entity;
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

    public class CargoController : BaseController
    {
        //
        // GET: /Intranet/Cargo/
        private ICargoRepository _cargoRepository;
        private INivelAcademicoCargoRepository _nivelAcademicoCargoRepository;
        private ICentroEstudioCargoRepository _centroEstudioCargoRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICompetenciaCargoRepository _competenciaCargoRepository;
        private IOfrecemosCargoRepository _ofrecemosCargoRepository;
        private IHorarioCargoRepository _horarioCargoRepository;
        private IUbigeoCargoRepository _ubigeoCargoRepository;
        private IUbigeoRepository _ubigeoRepository;
        private IConocimientoGeneralCargoRepository _conocimientoCargoRepository;
        private IExperienciaCargoRepository _experienciaCargoRepository;
        private IDiscapacidadCargoRepository _discapacidadCargoRepository;
        

        public CargoController(ICargoRepository cargoRepository,
                                INivelAcademicoCargoRepository nivelAcademicoRepository,
                                ICentroEstudioCargoRepository centroEstudiosRepository,
                                IDetalleGeneralRepository detalleGeneralRepository,
                                ICompetenciaCargoRepository competenciaCargoRepository,
                                IOfrecemosCargoRepository ofrecemosCargoRepository,
                                IHorarioCargoRepository horarioCargoRepository,
                                IUbigeoCargoRepository ubigeoCargoRepository,
                                IUbigeoRepository ubigeoRepository,
                                IConocimientoGeneralCargoRepository conocimientoCargoRepository,
                                IExperienciaCargoRepository experienciaCargoRepository,
                                IDiscapacidadCargoRepository discapacidadCargoRepository)
        {
            _cargoRepository = cargoRepository;
            _nivelAcademicoCargoRepository = nivelAcademicoRepository;
            _centroEstudioCargoRepository = centroEstudiosRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _competenciaCargoRepository = competenciaCargoRepository;
            _ofrecemosCargoRepository = ofrecemosCargoRepository;
            _horarioCargoRepository = horarioCargoRepository;
            _ubigeoCargoRepository = ubigeoCargoRepository;
            _ubigeoRepository = ubigeoRepository;
            _conocimientoCargoRepository = conocimientoCargoRepository;
            _experienciaCargoRepository = experienciaCargoRepository;
            _discapacidadCargoRepository = discapacidadCargoRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            Session["CargoIde"] = 1;
            var cargoViewModel = inicializarCargo();
            return View(cargoViewModel);
        }

        public CargoViewModel inicializarCargo()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            
            cargoViewModel.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            cargoViewModel.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            
            cargoViewModel.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            cargoViewModel.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            cargoViewModel.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return cargoViewModel;
        }

        [HttpPost]
        public ActionResult ListaCargo(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[11]
        {
          "200001",
          "200001",
          "Secretaría Ejecutiva",
          "Secretaría Ejecutiva",
          "Gerencia General",
          "Gerencia",
          "Gerencia",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[11]
        {
          "200002",
          "200002",
          "Técnico de Almacén",
          "Técnico de Almacén",
          "Logística",
          "Almacén",
          "Despacho",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[11]
        {
          "200003",
          "200003",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType2_4 = new
            {
                id = 4,
                cell = new string[11]
        {
          "200004",
          "200004",
          "Secretaría Ejecutiva",
          "Secretaría Ejecutiva",
          "Gerencia General",
          "Gerencia",
          "Gerencia",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_4);
            var fAnonymousType2_5 = new
            {
                id = 5,
                cell = new string[11]
        {
          "200005",
          "200005",
          "Técnico de Almacén",
          "Técnico de Almacén",
          "Logística",
          "Almacén",
          "Despacho",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_5);
            var fAnonymousType2_6 = new
            {
                id = 6,
                cell = new string[11]
        {
          "200006",
          "200006",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_6);
            var fAnonymousType2_7 = new
            {
                id = 7,
                cell = new string[11]
        {
          "200007",
          "200007",
          "Secretaría Ejecutiva",
          "Secretaría Ejecutiva",
          "Gerencia General",
          "Gerencia",
          "Gerencia",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_7);
            var fAnonymousType2_8 = new
            {
                id = 8,
                cell = new string[11]
        {
          "200008",
          "200008",
          "Técnico de Almacén",
          "Técnico de Almacén",
          "Logística",
          "Almacén",
          "Despacho",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_8);
            var fAnonymousType2_9 = new
            {
                id = 9,
                cell = new string[11]
        {
          "200009",
          "200009",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_9);
            var fAnonymousType2_10 = new
            {
                id = 10,
                cell = new string[11]
        {
          "200010",
          "200010",
          "Técnico en Enfermería",
          "Técnico en Enfermería",
          "Gerencia Medica",
          "Enfermería",
          "Cuidados Intensivos",
          "19/10/2012",
          "Admin",
          "23/10/2013",
          "Admin"        }
            };
            list.Add((object)fAnonymousType2_10);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        #region COMPETENCIA

        [HttpPost]
        public virtual JsonResult ListarCompetencias(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<CompetenciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_competenciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCompetenciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionCompetencia,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ViewResult Competencia()
        {
            var cargoViewModel = InicializarCompetencias();
            var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == 1);
            cargoViewModel.Competencia.Cargo = cargo;
            return View(cargoViewModel);
        }

        public CargoViewModel InicializarCompetencias()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Competencia = new CompetenciaCargo();

            cargoViewModel.Competencias = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCompetencia));
            cargoViewModel.Competencias.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            return cargoViewModel;
        }

        [HttpPost]
        public ActionResult Competencia([Bind(Prefix = "Competencia")]CompetenciaCargo competenciaCargo)
        {
            int IdeCargo = Convert.ToInt32(Session["CargoIde"]);
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    var competenciaViewModel = InicializarCompetencias();
                    competenciaViewModel.Competencia = competenciaCargo;
                    return View("Competencia", competenciaViewModel);
                }
                var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == IdeCargo);
                competenciaCargo.EstadoActivo = "A";
                competenciaCargo.FechaCreacion = FechaCreacion;
                competenciaCargo.UsuarioCreacion = "YO";
                competenciaCargo.FechaModificacion = FechaCreacion;
                //competenciaCargo.Cargo = new Cargo();
                //competenciaCargo.Cargo.IdeCargo = 1;
                cargo.agregarCompetencia(competenciaCargo);
                _competenciaCargoRepository.Add(competenciaCargo);

                objJsonMessage.Mensaje = "Agregado Correctamente";
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:"+ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }

        #endregion

        #region OFRECEMOS CARGO

        [HttpPost]
        public virtual JsonResult ListarOfrecemos(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<OfrecemosCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_ofrecemosCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeOfrecemosCargo.ToString(),
                        cell = new string[]
                            {
                                item.IdeOfrecemosCargo.ToString(),
                                item.DescripcionOfrecimiento,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ViewResult Ofrecemos()
        {
            var cargoViewModel = InicializarOfrecimientos();
            var cargo = _cargoRepository.GetSingle(x => x.IdeCargo == 1);
            cargoViewModel.Ofrecimiento.Cargo = cargo;
            return View(cargoViewModel);

        }

        public CargoViewModel InicializarOfrecimientos()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Ofrecimiento = new OfrecemosCargo();

            cargoViewModel.Ofrecimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoOfrecimiento));
            cargoViewModel.Ofrecimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        [HttpPost]
        public ActionResult Ofrecemos([Bind(Prefix = "Ofrecemos")]OfrecemosCargo ofrecemosCargo)
        {
            if (!ModelState.IsValid)
            {
                var OfrecemosViewModel = InicializarOfrecimientos();
                OfrecemosViewModel.Ofrecimiento = ofrecemosCargo;
                return View("Ofrecemos", OfrecemosViewModel);
            }
            _ofrecemosCargoRepository.Add(ofrecemosCargo);
            return View();

        }
        #endregion

        #region HORARIOS

        [HttpPost]
        public virtual JsonResult ListaHorario(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<HorarioCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_horarioCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeHorarioCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoHorario,
                                item.PuntajeHorario.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Horario()
        {
            var horariosViewModel = inicializarHorarios();
            return View(horariosViewModel);
        }

        [HttpPost]
        public ActionResult Horario([Bind(Prefix = "Horario")]HorarioCargo horarioCargo)
        {
            if (!ModelState.IsValid)
            {
                var horarioViewModel = inicializarHorarios();
                horarioViewModel.Horario = horarioCargo;
                return View("Horario", horarioViewModel);
            }
            _horarioCargoRepository.Add(horarioCargo);

            return View();

        }
        public CargoViewModel inicializarHorarios()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Horario = new HorarioCargo();

            cargoViewModel.Horarios = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));

            return cargoViewModel;
        }

        #endregion

        #region UBIGEO

        [HttpPost]
        public virtual JsonResult ListaUbigeo(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<UbigeoCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_ubigeoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeUbigeoCargo.ToString(),
                        cell = new string[]
                            {
                                item.IdeUbigeo.ToString(),
                                item.IdeUbigeo.ToString(),
                                item.IdeUbigeo.ToString(),
                                item.PuntajeUbigeo.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Ubigeo()
        {
            var ubigeoViewModel = inicializarUbigeos();
            return View(ubigeoViewModel);
        }

        [HttpPost]
        public ActionResult Ubigeo([Bind(Prefix = "Ubigeo")]UbigeoCargo ubigeoCargo)
        {
            if (!ModelState.IsValid)
            {
                var ubigeoViewModel = inicializarUbigeos();
                ubigeoViewModel.Ubigeo = ubigeoCargo;
                return View("Ubigeo", ubigeoViewModel);
            }
            _ubigeoCargoRepository.Add(ubigeoCargo);

            return View();

        }
        public CargoViewModel inicializarUbigeos()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Ubigeo = new UbigeoCargo();

            cargoViewModel.Departamentos = new List<Ubigeo>(_ubigeoRepository.GetBy(x=>x.IdeUbigeoPadre == null));
            cargoViewModel.Departamentos.Insert(0, new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            cargoViewModel.Provincias = new List<Ubigeo>();
            cargoViewModel.Provincias.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            cargoViewModel.Distritos = new List<Ubigeo>();
            cargoViewModel.Distritos.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            return cargoViewModel;
        }

        [HttpPost]
        public ActionResult listarUbigeos(int ideUbigeoPadre)
        {
            ActionResult result = null;

            var listaResultado = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == ideUbigeoPadre));
            result = Json(listaResultado);
            return result;
        }

        #endregion

        #region NIVEL ACADEMICO

        [HttpPost]
        public virtual JsonResult ListaNivelAcademico(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<NivelAcademicoCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_nivelAcademicoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoEducacion.ToString(),
                                item.TipoAreaEstudio.ToString(),
                                item.TipoNivelAlcanzado.ToString(),
                                item.PuntajeNivelEstudio.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult NivelAcademico()
        {
            var nivelAcademicoViewModel = inicializarNivelAcademico();
            return View(nivelAcademicoViewModel);
        }

        [HttpPost]
        public ActionResult Ubigeo([Bind(Prefix = "NivelAcademico")]NivelAcademicoCargo nivelAcademicoCargo)
        {
            if (!ModelState.IsValid)
            {
                var nivelAcademicoViewModel = inicializarNivelAcademico();
                nivelAcademicoViewModel.NivelAcademico  = nivelAcademicoCargo;
                return View("NivelAcademico", nivelAcademicoViewModel);
            }
            _nivelAcademicoCargoRepository.Add(nivelAcademicoCargo);

            return View();

        }

        public CargoViewModel inicializarNivelAcademico()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.NivelAcademico = new NivelAcademicoCargo();

            cargoViewModel.TiposEducacion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEducacion));
            cargoViewModel.TiposEducacion.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.AreasEstudio = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoArea));
            cargoViewModel.AreasEstudio.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.NivelesAlcanzados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.NivelAlcanzado));
            cargoViewModel.NivelesAlcanzados.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            
            return cargoViewModel;
        }

        #endregion

        #region CENTRO ESTUDIOS

        [HttpPost]
        public virtual JsonResult ListaCentroEstudio(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<CentroEstudioCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_centroEstudioCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCentroEstudioCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoCentroEstudio,
                                item.TipoNombreCentroEstudio,
                                item.PuntajeCentroEstudios.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult CentroEstudio()
        {
            var centroEstudioViewModel = inicializarCentroEstudio();
            return View(centroEstudioViewModel);
        }

        [HttpPost]
        public ActionResult CentroEstudio([Bind(Prefix = "CentroEstudio")]CentroEstudioCargo centroEstudioCargo)
        {
            if (!ModelState.IsValid)
            {
                var centroEstudioViewModel = inicializarCentroEstudio();
                centroEstudioViewModel.CentroEstudio = centroEstudioCargo;
                return View("CentroEstudio", centroEstudioViewModel);
            }
            _centroEstudioCargoRepository.Add(centroEstudioCargo);

            return View();

        }
        public CargoViewModel inicializarCentroEstudio()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.CentroEstudio = new CentroEstudioCargo();

            cargoViewModel.TiposInstitucion = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoInstitucion));
            cargoViewModel.TiposInstitucion.Insert(0, new DetalleGeneral { Valor= "00", Descripcion = "Seleccionar"});

            cargoViewModel.Instituciones = new List<DetalleGeneral>();
            cargoViewModel.Instituciones.Add(new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

        #region OFIMATICA

        [HttpPost]
        public virtual JsonResult ListaOfimatica(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoConocimientoOfimatica.ToString(),
                                item.TipoNombreOfimatica.ToString(),
                                item.TipoNivelConocimiento.ToString(),
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Ofimatica()
        {
            var ofimaticaViewModel = inicializarOfimatica();
            return View(ofimaticaViewModel);
        }

        [HttpPost]
        public ActionResult Ofimatica([Bind(Prefix = "Conocimiento")]ConocimientoGeneralCargo conocimientoGeneralCargo)
        {
            if (!ModelState.IsValid)
            {
                var conocimientoCargoViewModel = inicializarOfimatica();
                conocimientoCargoViewModel.Conocimiento = conocimientoGeneralCargo;
                return View("Conocimiento", conocimientoGeneralCargo);
            }
            _conocimientoCargoRepository.Add(conocimientoGeneralCargo);

            return View();

        }

        public CargoViewModel inicializarOfimatica()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Conocimiento = new ConocimientoGeneralCargo();

            cargoViewModel.TipoConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoOfimatica));
            cargoViewModel.TipoConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.DescripcionConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TiponombreOfimatica));
            cargoViewModel.DescripcionConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.NivelesConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            cargoViewModel.NivelesConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

        #region IDIOMA

        [HttpPost]
        public virtual JsonResult ListaIdioma(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoIdioma.ToString(),
                                item.TipoConocimientoIdioma.ToString(),
                                item.TipoNivelConocimiento.ToString(),
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Idioma()
        {
            var idiomaViewModel = inicializarIdioma();
            return View(idiomaViewModel);
        }

        [HttpPost]
        public ActionResult Idioma([Bind(Prefix = "Conocimiento")]ConocimientoGeneralCargo conocimientoCargo)
        {
            if (!ModelState.IsValid)
            {
                var idiomaViewModel = inicializarIdioma();
                idiomaViewModel.Conocimiento  = conocimientoCargo;
                return View("Conocimiento", idiomaViewModel);
            }
            _conocimientoCargoRepository.Add(conocimientoCargo);

            return View();

        }

        public CargoViewModel inicializarIdioma()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Conocimiento = new ConocimientoGeneralCargo();

            cargoViewModel.DescripcionConocimiento = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoIdioma));
            cargoViewModel.DescripcionConocimiento.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.TipoConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoIdioma));
            cargoViewModel.TipoConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.NivelesConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            cargoViewModel.NivelesConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

        #region OTROS CONOCIMIENTOS

        [HttpPost]
        public virtual JsonResult ListaOtrosConocimientos(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_conocimientoCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoConocimientoGeneral.ToString(),
                                item.TipoNombreConocimientoGeneral.ToString(),
                                item.TipoNivelConocimiento.ToString(),
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult OtrosConocimientos()
        {
            var otrosConocimientodViewModel = inicializarOtrosConocimientos();
            return View(otrosConocimientodViewModel);
        }

        [HttpPost]
        public ActionResult OtrosConocimientos([Bind(Prefix = "Conocimiento")]ConocimientoGeneralCargo conocimientoCargo)
        {
            if (!ModelState.IsValid)
            {
                var otrosConocimientodViewModel = inicializarOtrosConocimientos();
                otrosConocimientodViewModel.Conocimiento = conocimientoCargo;
                return View("NivelAcademico", otrosConocimientodViewModel);
            }
            _conocimientoCargoRepository.Add(conocimientoCargo);

            return View();

        }

        public CargoViewModel inicializarOtrosConocimientos()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Conocimiento = new ConocimientoGeneralCargo();

            cargoViewModel.TipoConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoConocimientoGral));
            cargoViewModel.TipoConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.DescripcionConocimiento = new List<DetalleGeneral>();
            cargoViewModel.DescripcionConocimiento.Add(new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.NivelesConocimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoNivelConocimiento));
            cargoViewModel.NivelesConocimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

        #region EXPERIENCIA

        [HttpPost]
        public virtual JsonResult ListaExperiencia(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ExperienciaCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_experienciaCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoExperiencia.ToString(),
                                item.CantidadAnhosExperiencia.ToString(),
                                item.CantidadMesesExperiencia.ToString(),
                                item.PuntajeExperiencia.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Experiencia()
        {
            var experienciaViewModel = inicializarExperiencia();
            return View(experienciaViewModel);
        }

        [HttpPost]
        public ActionResult Idioma([Bind(Prefix = "Experiencia")]ExperienciaCargo experienciaCargo)
        {
            if (!ModelState.IsValid)
            {
                var experienciaViewModel = inicializarExperiencia();
                experienciaViewModel.Experiencia = experienciaCargo;
                return View("Experiencia", experienciaViewModel);
            }
            _experienciaCargoRepository.Add(experienciaCargo);

            return View();

        }

        public CargoViewModel inicializarExperiencia()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Experiencia = new ExperienciaCargo();

            cargoViewModel.TipoCargos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoCargo));
            cargoViewModel.TipoCargos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion

        #region DISCAPACIDAD

        [HttpPost]
        public virtual JsonResult ListaDiscapacidad(GridTable grid)
        {
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<DiscapacidadCargo>();
                where.Add(Expression.Eq("Cargo.IdeCargo", 1));

                var generic = Listar(_discapacidadCargoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeDiscapacidadCargo.ToString(),
                        cell = new string[]
                            {
                                item.TipoDiscapacidad.ToString(),
                                item.PuntajeDiscapacidad.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        public ActionResult Discapacidad()
        {
            var discapacidadViewModel = inicializarDiscapacidad();
            return View(discapacidadViewModel);
        }

        [HttpPost]
        public ActionResult Discapacidad([Bind(Prefix = "Discapacidad")]DiscapacidadCargo discapacidadCargo)
        {
            if (!ModelState.IsValid)
            {
                var discapacidadViewModel = inicializarDiscapacidad();
                discapacidadViewModel.Discapacidad = discapacidadCargo;
                return View("Discapacidad", discapacidadViewModel);
            }
            _discapacidadCargoRepository.Add(discapacidadCargo);

            return View();

        }

        public CargoViewModel inicializarDiscapacidad()
        {
            var cargoViewModel = new CargoViewModel();
            cargoViewModel.Cargo = new Cargo();
            cargoViewModel.Discapacidad = new DiscapacidadCargo();

            cargoViewModel.TipoDiscapacidad = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoDiscapacidad));
            cargoViewModel.TipoDiscapacidad.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            return cargoViewModel;
        }

        #endregion
 
    }
}
