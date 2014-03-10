﻿

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
    
    
    [Authorize]
    public class SolicitudCargoController : BaseController
    {


        /// <summary>
        /// Se inicializa los repositorios de base de datos
        /// </summary>
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IAreaRepository _areaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;
        private ITipoRequerimiento _tipoRequerimiento;
        private ICargoRepository _cargoRepository;
        private IConocimientoGeneralRequerimientoRepository _ConocimientoGeneralRequerimientoRepository;
        private INivelAcademicoRequerimientoRepository _nivelAcademicoRequerimientoRepository;
        private ICompetenciaRequerimientoRepository _competenciaRequerimientoRepository;
        private IExperienciaRequerimientoRepository _experienciaRequerimientoRepository;
        private IOfrecemosRequerimientoRepository _ofrecemosRequerimientoRepository;
      
        public SolicitudCargoController( IDetalleGeneralRepository detalleGeneralRepository,
                                         ISolReqPersonalRepository solReqPersonalRepository,
                                         IDependenciaRepository dependenciaRepository,
                                         IAreaRepository areaRepository,
                                         IDepartamentoRepository departamentoRepository,
                                         IUsuarioRolSedeRepository usuarioRolSedeRepository,
                                         ITipoRequerimiento tipoRequerimiento,
                                         ICargoRepository cargoRepository,
            IConocimientoGeneralRequerimientoRepository conocimientoGeneralRequerimientoRepository,
            INivelAcademicoRequerimientoRepository nivelAcademicoRequerimientoRepository,
            ICompetenciaRequerimientoRepository CompetenciaRequerimientoRepository,
            IExperienciaRequerimientoRepository experienciaRequerimientoRepository,
            IOfrecemosRequerimientoRepository ofrecemosRequerimientoRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _dependenciaRepository = dependenciaRepository;
            _areaRepository = areaRepository;
            _departamentoRepository = departamentoRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _tipoRequerimiento = tipoRequerimiento;
            _cargoRepository = cargoRepository;
            _ConocimientoGeneralRequerimientoRepository = conocimientoGeneralRequerimientoRepository;
            _nivelAcademicoRequerimientoRepository = nivelAcademicoRequerimientoRepository;
            _competenciaRequerimientoRepository = CompetenciaRequerimientoRepository;
            _experienciaRequerimientoRepository = experienciaRequerimientoRepository;
            _ofrecemosRequerimientoRepository = ofrecemosRequerimientoRepository;
        }

        




        #region Nuevo

        public ActionResult ListaNuevo()
        {
            return View();
        }

        public ActionResult CrearNuevo() 
        {
            return View("InformacionNuevo");
        }
        [HttpPost]
        public ActionResult ListaSolicitudPersonal(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "200002",
          "200002",
          "Técnico de Almacén",
          "Gerencia Administrativa",
          "Logística",
          "Almacén",
          "5",
          "8",
          "0",
          "0",
          "0",
          "0",
          "20/10/2012",
          "",
          "RRHH",
          "Pedro Gutierrez",
          "SI",
          "Pend. Aproba.",
          "Almacén - Técnico de Almacén",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_5 = new
            {
                id = 5,
                cell = new string[]
        {
          "200005",
          "200005",
          "Técnico de Almacén",
          "Gerencia Administrativa",
          "Logística",
          "Almacén",
          "8",
          "8",
          "0",
          "0",
          "0",
          "0",
          "20/10/2012",
          "",
          "RRHH",
          "Pedro Gutierrez",
          "SI",
          "Pend. Aproba.",
          "Almacén - Técnico de Almacén",
          "Activo"
        }
            };

            list.Add((object)fAnonymousType2_5);
            var fAnonymousType2_8 = new
            {
                id = 8,
                cell = new string[]
        {
          "200008",
          "200008",
          "Técnico de Almacén",
          "Gerencia Administrativa",
          "Logística",
          "Almacén",
          "8",
          "8",
          "0",
          "0",
          "0",
          "0",
          "20/10/2012",
          "",
          "RRHH",
          "Pedro Gutierrez",
          "SI",
          "Pend. Aproba.",
          "Almacén - Técnico de Almacén",
          "Activo"
        }
            };

            list.Add((object)fAnonymousType2_8);


            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "200001",
          "200001",
          "Secretaría Ejecutiva",
          "Gerencia de Créditos",
          "Cobranzas",
          "Cobranzas",
          "5",
          "0",
          "0",
          "0",
          "0",
          "0",
          "19/10/2012",
          "",
          "Jefe de Cobranza",
          "Carol Sandoval",
          "NO",
          "Nuevo",
          "Cobranzas - Secretaría Ejecutiva",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_1);

            var fAnonymousType2_4 = new
            {
                id = 4,
                cell = new string[]
        {
          "200004",
          "200004",
          "Secretaría Ejecutiva",
          "Gerencia de Créditos",
          "Cobranzas",
          "Cobranzas",
          "6",
          "0",
          "0",
          "0",
          "0",
          "0",
          "19/10/2012",
          "",
          "Jefe de Cobranza",
          "Carol Sandoval",
          "NO",
          "Nuevo",
          "Cobranzas - Secretaría Ejecutiva",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_4);

            var fAnonymousType2_7 = new
            {
                id = 7,
                cell = new string[]
        {
          "200007",
          "200007",
          "Secretaría Ejecutiva",
          "Gerencia de Créditos",
          "Cobranzas",
          "Cobranzas",
          "5",
          "0",
          "0",
          "0",
          "0",
          "0",
          "19/10/2012",
          "",
          "Jefe de Cobranza",
          "Carol Sandoval",
          "NO",
          "Nuevo",
          "Cobranzas - Secretaría Ejecutiva",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_7);

            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[]
        {
          "200003",
          "200003",
          "Técnico de Enfermería",
          "Gerencia Médica",
          "Enfermería",
          "Cuidados Intensivos",
          "4",
          "1",
          "50",
          "2",
          "1",
          "1",
          "20/10/2012",
          "",
          "RRHH",
          "Pedro Gutierrez",
          "SI",
          "En Evaluación",
          "Cuidados Intensivos - Técnico de Enfermería",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_3);


            var fAnonymousType2_6 = new
            {
                id = 6,
                cell = new string[]
        {
          "200006",
          "200006",
          "Técnico de Enfermería",
          "Gerencia Médica",
          "Enfermería",
          "Cuidados Intensivos",
          "5",
          "1",
          "50",
          "2",
          "1",
          "1",
          "20/10/2012",
          "",
          "RRHH",
          "Pedro Gutierrez",
          "SI",
          "En Evaluación",
          "Cuidados Intensivos - Técnico de Enfermería",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_6);


            var fAnonymousType2_9 = new
            {
                id = 9,
                cell = new string[]
        {
          "200009",
          "200009",
          "Técnico de Enfermería",
          "Gerencia Médica",
          "Enfermería",
          "Cuidados Intensivos",
          "5",
          "1",
          "50",
          "2",
          "1",
          "1",
          "20/10/2012",
          "",
          "RRHH",
          "Pedro Gutierrez",
          "SI",
          "En Evaluación",
          "Cuidados Intensivos - Técnico de Enfermería",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_9);
            var fAnonymousType2_10 = new
            {
                id = 10,
                cell = new string[]
        {
          "200010",
          "200010",
          "Técnico de Enfermería",
          "Gerencia Médica",
          "Enfermería",
          "Cuidados Intensivos",
          "5",
          "1",
          "50",
          "2",
          "1",
          "1",
          "20/10/2012",
          "",
          "RRHH",
          "Pedro Gutierrez",
          "SI",
          "En Evaluación",
          "Cuidados Intensivos - Técnico de Enfermería",
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_10);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaEstudios(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2 = new
            {
                id = 1,
                cell = new string[]
        {
          "200104",
          "Técnico ",
          "Enfermería",
          "Titulado",
          "15"
        }
            };
            list.Add((object)fAnonymousType2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaCompetencias(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "C0001",
          "Responsabilidad"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "C0002",
          "Capacidad para resolver problemas"
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaFunciones(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2 = new
            {
                id = 1,
                cell = new string[]
        {
          "F0001",
          "Coordinar con los Médicos",
          "10"
        }
            };
            list.Add((object)fAnonymousType2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        #endregion

        #region Ampliacion

        [AuthorizeUser]
        public ActionResult ListaAmpliacion()
        {
            return View();
        }

        public ActionResult CrearAmpliacion()
        {
            return View("InformacionAmpliacion");
        }

        [HttpPost]
        public ActionResult ListaSolicitudAmpliacion(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var item = new
            {
                id = 1,
                cell = new string[]
				{
					"200001",
					"200001",
					"Secretaría Ejecutiva",
					"Gerencia de Créditos",
					"Cobranzas",
					"Cobranzas",
					"0",
					"0",
					"0",
					"0",
					"0",
					"19/10/2012",
					"",
					"Jefe de Cobranza",
					"Carol Sandoval",
					"NO",
					"Nuevo",
					"Activo"
				}
            };
            list.Add(item);
            var item2 = new
            {
                id = 2,
                cell = new string[]
				{
					"200002",
					"200002",
					"Técnico de Almacén",
					"Gerencia Administrativa",
					"Logística",
					"Almacén",
					"8",
					"0",
					"0",
					"0",
					"0",
					"20/10/2012",
					"",
					"RRHH",
					"Pedro Gutierrez",
					"SI",
					"Pend. Aproba.",
					"Activo"
				}
            };
            list.Add(item2);
            var item3 = new
            {
                id = 3,
                cell = new string[]
				{
					"200003",
					"200003",
					"Técnico de Enfermería",
					"Gerencia Médica",
					"Enfermería",
					"Cuidados Intensivos",
					"1",
					"50",
					"2",
					"1",
					"1",
					"20/10/2012",
					"",
					"RRHH",
					"Pedro Gutierrez",
					"SI",
					"En Evaluación",
					"Activo"
				}
            };
            list.Add(item3);
            var item4 = new
            {
                id = 4,
                cell = new string[]
				{
					"200004",
					"200004",
					"Secretaría Ejecutiva",
					"Gerencia de Créditos",
					"Cobranzas",
					"Cobranzas",
					"0",
					"0",
					"0",
					"0",
					"0",
					"19/10/2012",
					"",
					"Jefe de Cobranza",
					"Carol Sandoval",
					"NO",
					"Nuevo",
					"Activo"
				}
            };
            list.Add(item4);
            var item5 = new
            {
                id = 5,
                cell = new string[]
				{
					"200005",
					"200005",
					"Técnico de Almacén",
					"Gerencia Administrativa",
					"Logística",
					"Almacén",
					"8",
					"0",
					"0",
					"0",
					"0",
					"20/10/2012",
					"",
					"RRHH",
					"Pedro Gutierrez",
					"SI",
					"Pend. Aproba.",
					"Activo"
				}
            };
            list.Add(item5);
            var item6 = new
            {
                id = 6,
                cell = new string[]
				{
					"200006",
					"200006",
					"Técnico de Enfermería",
					"Gerencia Médica",
					"Enfermería",
					"Cuidados Intensivos",
					"1",
					"50",
					"2",
					"1",
					"1",
					"20/10/2012",
					"",
					"RRHH",
					"Pedro Gutierrez",
					"SI",
					"En Evaluación",
					"Activo"
				}
            };
            list.Add(item6);
            var item7 = new
            {
                id = 7,
                cell = new string[]
				{
					"200007",
					"200007",
					"Secretaría Ejecutiva",
					"Gerencia de Créditos",
					"Cobranzas",
					"Cobranzas",
					"0",
					"0",
					"0",
					"0",
					"0",
					"19/10/2012",
					"",
					"Jefe de Cobranza",
					"Carol Sandoval",
					"NO",
					"Nuevo",
					"Activo"
				}
            };
            list.Add(item7);
            var item8 = new
            {
                id = 8,
                cell = new string[]
				{
					"200008",
					"200008",
					"Técnico de Almacén",
					"Gerencia Administrativa",
					"Logística",
					"Almacén",
					"8",
					"0",
					"0",
					"0",
					"0",
					"20/10/2012",
					"",
					"RRHH",
					"Pedro Gutierrez",
					"SI",
					"Pend. Aproba.",
					"Activo"
				}
            };
            list.Add(item8);
            var item9 = new
            {
                id = 9,
                cell = new string[]
				{
					"200009",
					"200009",
					"Técnico de Enfermería",
					"Gerencia Médica",
					"Enfermería",
					"Cuidados Intensivos",
					"1",
					"50",
					"2",
					"1",
					"1",
					"20/10/2012",
					"",
					"RRHH",
					"Pedro Gutierrez",
					"SI",
					"En Evaluación",
					"Activo"
				}
            };
            list.Add(item9);
            var item10 = new
            {
                id = 10,
                cell = new string[]
				{
					"200010",
					"200010",
					"Técnico de Enfermería",
					"Gerencia Médica",
					"Enfermería",
					"Cuidados Intensivos",
					"1",
					"50",
					"2",
					"1",
					"1",
					"20/10/2012",
					"",
					"RRHH",
					"Pedro Gutierrez",
					"SI",
					"En Evaluación",
					"Activo"
				}
            };
            list.Add(item10);
            var data = new
            {
                rows = list
            };
            return base.Json(data);
        }

        [HttpPost]
        public ActionResult ListaOfrecemos(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var item = new
            {
                id = 1,
                cell = new string[]
				{
					"O0001",
					"Remuneración promedio al mercado"
				}
            };
            list.Add(item);
            var item2 = new
            {
                id = 2,
                cell = new string[]
				{
					"O0002",
					"Ingreso a planilla con todos los beneficios de ley"
				}
            };
            list.Add(item2);
            var data = new
            {
                rows = list
            };
            return base.Json(data);
        }

        [HttpPost]
        public ActionResult ListaHorario(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2 = new
            {
                id = 1,
                cell = new string[]
        {
          "0001",
          "Tiempo Completo",
          "20"
        }
            };
            list.Add((object)fAnonymousType2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaUbigeo(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2 = new
            {
                id = 1,
                cell = new string[]
        {
          "150136",
          "Lima",
          "Lima",
          "San Miguel",
          "15"
        }
            };
            list.Add((object)fAnonymousType2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaCentroEstudios(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2 = new
            {
                id = 1,
                cell = new string[]
        {
          "10013",
          "Universidad",
          "Universidad Católica del Perú",
          "20"
        }
            };
            list.Add((object)fAnonymousType2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaExperienciaLaboral(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2 = new
            {
                id = 1,
                cell = new string[]
        {
          "31405",
          "Analista de Sistemas",
          "3",
          "20"
        }
            };
            list.Add((object)fAnonymousType2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaConocimientosGeneralaes(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2 = new
            {
                id = 1,
                cell = new string[]
        {
          "5201",
          "Software",
          "Microsoft Word",
          "Avanzado",
          "15"
        }
            };
            list.Add((object)fAnonymousType2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaConocimientosIdiomas(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2 = new
            {
                id = 1,
                cell = new string[]
        {
          "4001",
          "Inglés                ",
          "Escritura            ",
          "Avanzado",
          "20"
        }
            };
            list.Add((object)fAnonymousType2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaDiscapacidades(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2 = new
            {
                id = 1,
                cell = new string[]
        {
          "15001",
          "Motora",
          "Parálisis pierna izquierda",
          "20"
        }
            };
            list.Add((object)fAnonymousType2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }

        [HttpPost]
        public ActionResult ListaEvaluacionesPerfil(string sidx, string sord, int page, int rows)
        {
            List<object> list = new List<object>();
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[]
        {
          "15305",
          "Examen 3",
          "Examen",
          "11",
          "RRHH",
          "50"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[]
        {
          "15309",
          "Examen 4",
          "Entrevista",
          "12",
          "GA",
          "40"
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType3 = new
            {
                rows = list
            };
            return (ActionResult)this.Json((object)fAnonymousType3);
        }
        #endregion 

        #region Reemplazo

        /// <summary>
        /// inicializa la busqueda de lista de reemplazo
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult ListaReemplazo()
        {
            SolicitudRempCargoViewModel model;
            try
            {
                model = new SolicitudRempCargoViewModel();
                

                var sede = Session[ConstanteSesion.Sede];
                if (sede!=null)
                {
                  
                  model = InicializarListaReemplazo(Convert.ToInt32(sede));
                  model.SolReqPersonal = new SolReqPersonal();

                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
            return View("ListaReemplazo", model);
        }

        /// <summary>
        /// lista de departamentos
        /// </summary>
        /// <param name="ideDependencia"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaDepartamentos(int ideDependencia)
        {
            ActionResult result = null;
            Dependencia objDepencia = new Dependencia();

            var listaResultado = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == ideDependencia));
            result = Json(listaResultado);
            return result;
        }

        /// <summary>
        /// lista de areas
        /// </summary>
        /// <param name="ideDepartamento"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaAreas(int ideDepartamento)
        {
            ActionResult result = null;

            var listaResultado = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento));
            result = Json(listaResultado);
            return result;
        }

        /// <summary>
        /// iniciliza la pantalla de busqueda de reemplazo
        /// </summary>
        /// <param name="idSel"></param>
        /// <returns></returns>
        public SolicitudRempCargoViewModel InicializarListaReemplazo(int idSel)
        {
            var model = new SolicitudRempCargoViewModel();

            model.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                         && x.IdeSede == idSel));
            model.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            model.Departamentos = new List<Departamento>();
            model.Departamentos.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            model.Areas = new List<Area>();
            model.Areas.Add(new Area { IdeArea = 0, NombreArea = "Seleccionar" });

            model.listaTipCargo = new List<Cargo>(_solReqPersonalRepository.GetTipCargo(0));
            model.listaTipCargo.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            model.listaRol = new List<Rol>(_usuarioRolSedeRepository.GetListaRol(0));
            model.listaRol.Insert(0, new Rol { IdRol = 0, CodRol = "Seleccionar" });

            model.listaEtapas =
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapaSolicitud));
            model.listaEtapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.listaEstados =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoMant));
            model.listaEstados.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.listaTipPuesto =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            model.listaTipPuesto.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return model;
        }


        /// <summary>
        /// Lista de busqueda de reemplazo
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListBusquedaReemplazo(GridTable grid)
        {

            SolReqPersonal solReqPersonal;
            List<SolReqPersonal> lista = new List<SolReqPersonal>();
            try
            {
               
                    solReqPersonal = new SolReqPersonal();

                    solReqPersonal.IdeCargo = (grid.rules[1].data==null?0:Convert.ToInt32(grid.rules[1].data));
                    solReqPersonal.IdeDependencia = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));
                    solReqPersonal.IdeArea = (grid.rules[3].data == null ? 0 : Convert.ToInt32(grid.rules[3].data));
                    solReqPersonal.TipResponsable = (grid.rules[4].data == null ? "" : grid.rules[4].data);

                    if (grid.rules[5].data!=null && grid.rules[6].data!=null)
                    {
                        solReqPersonal.FechaInicioBus = Convert.ToDateTime(grid.rules[5].data);
                        solReqPersonal.FechaFinBus = Convert.ToDateTime(grid.rules[6].data);      
                    }
                  
                    solReqPersonal.IdeDepartamento = (grid.rules[7].data == null ? 0 : Convert.ToInt32(grid.rules[7].data));
                    solReqPersonal.TipEtapa = (grid.rules[8].data == null ? "" : grid.rules[8].data);
                    solReqPersonal.TipEstado = (grid.rules[9].data == null ? "" : grid.rules[9].data);

                    lista = _solReqPersonalRepository.GetListaSolReqPersonal(solReqPersonal);
               


                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);
               
                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeSolReqPersonal.ToString(),
                    cell = new string[]
                            {
                               
                                "1",
                                item.TipEstado==null?"":item.TipEstado,
                                item.IdeSolReqPersonal==null?"":item.IdeSolReqPersonal.ToString(),
                                item.CodSolReqPersonal==null?"":item.CodSolReqPersonal.ToString(),
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.DesCargo==null?"":item.DesCargo,
                                item.IdeDependencia==null?"":item.IdeDependencia.ToString(),
                                item.Dependencia_des==null?"":item.Dependencia_des,
                                item.IdeDepartamento==null?"":item.IdeDepartamento.ToString(),
                                item.Departamento_des==null?"":item.Departamento_des,
                                item.IdeArea==null?"":item.IdeArea.ToString(),
                                item.Area_des==null?"":item.Area_des,
                                item.NumVacantes==null?"":item.NumVacantes.ToString(),
                                item.CantPostulante==null?"":item.CantPostulante.ToString(),
                                item.CantPreSelec==null?"":item.CantPreSelec.ToString(),
                                item.CantEvaluados==null?"":item.CantEvaluados.ToString(),
                                item.CantSeleccionados==null?"":item.CantSeleccionados.ToString(),
                                item.Feccreacion==null?"":item.Feccreacion.ToString(),
                                item.FecExpiracacion==null?"":item.FecExpiracacion.ToString(),
                               
                                item.idRolSuceso==null?"":item.idRolSuceso.ToString(),
                                item.DesRolSuceso==null?"":item.DesRolSuceso,
                                item.NomPersonReemplazo==null?"":item.NomPersonReemplazo,
                                
                                item.FlagPublicado==null?"":item.FlagPublicado,
                                item.TipEtapa==null?"":item.TipEtapa,
                                item.idUsuarioResp ==null?"":item.idUsuarioResp.ToString()
                               
                            }
                }).ToArray();

                return Json(generic.Value);
                
            }
            catch (Exception ex)
            {
               
                return MensajeError();
            }
        }



        /// <summary>
        /// pagina para crear un nuevo reemplazo
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult CrearReemplazo()
        {
            SolicitudRempCargoViewModel model;
            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            SedeNivel UsuarioSede;
            string Nombusuario=null;
            string codCodificado = null;
            string CodGenerado=null;

            DateTime hoy = DateTime.Today;
            
            model = new SolicitudRempCargoViewModel();
            
            var objUsuarioSede = Session[ConstanteSesion.UsuarioSede];

            if (objUsuarioSede!=null)
            {
                UsuarioSede = new SedeNivel();
                UsuarioSede = (SedeNivel)objUsuarioSede;
                model = Inicializar(UsuarioSede);
                
                model.SolReqPersonal = new SolReqPersonal();

                if (idRol!=Roles.Gerente && idRol!=Roles.Gerente_General_Adjunto)
                {
                    model.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                        && x.IdeSede == UsuarioSede.IDESEDE
                                                                        && x.IdeDependencia == UsuarioSede.IDEDEPENDENCIA
                                                                        ));
            

                    model.listaDepartamento = new List<Departamento>(_departamentoRepository.GetBy(x => x.IdeDepartamento == UsuarioSede.IDEDEPARTAMENTO
                                                                                              && x.Dependencia.IdeDependencia == UsuarioSede.IDEDEPENDENCIA
                                                                                              && x.EstadoActivo == IndicadorActivo.Activo));


                    model.listaArea = new List<Area>(_areaRepository.GetBy(x => x.IdeArea == UsuarioSede.IDEAREA
                                                                              && x.Departamento.IdeDepartamento == UsuarioSede.IDEDEPARTAMENTO
                                                                              && x.EstadoActivo == IndicadorActivo.Activo));

                }
                
            }
            
           

            model.Accion = Accion.Nuevo;
            var codUsuario = Convert.ToString(Session[ConstanteSesion.Usuario]);
            var codSede = Convert.ToString(Session[ConstanteSesion.Sede]);
            var codRol = Convert.ToString(Session[ConstanteSesion.Rol]);

            CodGenerado = codUsuario+codSede+codRol+String.Format("{0:MM/dd/yyyy}", hoy) + System.Guid.NewGuid().ToString();
            
            codCodificado = Base64Encode(CodGenerado);

            Session.Remove(ConstanteSesion.ListaReemplazo);
            Session.Remove(ConstanteSesion.codReqSolTemp);
            
            Session[ConstanteSesion.codReqSolTemp] = codCodificado;


            model.Accion = Accion.Enviar;


            return View("InformacionReemplazo", model);
        }

        /// <summary>
        /// Inicializa la pagina inicial de reemplazo
        /// </summary>
        /// <returns></returns>
       
        private SolicitudRempCargoViewModel Inicializar(SedeNivel UsuarioSede)
        {
            var objModel = new SolicitudRempCargoViewModel();
            objModel.SolReqPersonal = new SolReqPersonal();
           
            objModel.listaTipVacante=
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoVacante));
            objModel.listaTipVacante.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            objModel.listaTipPuesto =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            objModel.listaTipPuesto.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });


            objModel.listaTipCargo = new List<Cargo>(_solReqPersonalRepository.GetTipCargo(0));
            objModel.listaTipCargo.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });


          


            objModel.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                         && x.IdeSede == UsuarioSede.IDESEDE));

            objModel.listaDependencia.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            objModel.listaDepartamento = new List<Departamento>();
            objModel.listaDepartamento.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            objModel.listaArea = new List<Area>();
            objModel.listaArea.Add(new Area { IdeArea = 0, NombreArea = "Seleccionar" });

            return objModel;
        }


        



       /// <summary>
       /// Incializa el popup de reemplazo
       /// </summary>
       /// <returns></returns>
        public ActionResult InicioPopupReemplazo(string id) 
        {

            SolicitudRempCargoViewModel model;

            model = new SolicitudRempCargoViewModel();
            model.SolReqPersonal = new SolReqPersonal();

            model.Reemplazo = new Reemplazo();

            DateTime Hoy = DateTime.Today;

            if (id != null && id.Length > 0)
            {
                model.SolReqPersonal.CodSolReqPersonal = id;
            }
            
            
            
            return View("PopupListaReemplazo",model);
        }
        
        /// <summary>
        /// obtiene los valores del popup
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetPopupReemplazo(SolicitudRempCargoViewModel model) 
        {

            JsonMessage objJson = new JsonMessage();
            Reemplazo objReemplazo;
            objReemplazo = new Reemplazo();
            objReemplazo = model.Reemplazo;


            var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == model.SolReqPersonal.CodSolReqPersonal);


            objReemplazo.IdeSolReqPersonal = Convert.ToInt32(objSol.IdeSolReqPersonal);
            objReemplazo.FechaCreacion = FechaSistema;
            objReemplazo.UsuarioCreacion = UsuarioActual.NombreUsuario;

            int dato = 0;
            dato = _solReqPersonalRepository.InsertTempReemplazo(objReemplazo);

            objJson.Mensaje = "Se grabo el registro correctamente";
            objJson.Resultado = true;

            return Json(objJson);
        }

        /// <summary>
        /// muestra la pantalla del requerimiento solo con la opcion de aprobar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Edit(string id) {
            SolicitudRempCargoViewModel model;
            model = new SolicitudRempCargoViewModel();
            string EtapaSol = null;

            var ObjSol = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(id));
            EtapaSol = ObjSol.TipEtapa;
            
            var idRolUsuario = Convert.ToInt32(Session[ConstanteSesion.Rol]);

            if (ObjSol!=null)
            {
                SedeNivel sedeNivel = new SedeNivel();

                sedeNivel.IDESEDE = ObjSol.IdeSede;

                model = Inicializar(sedeNivel);
                model.SolReqPersonal = new SolReqPersonal();
                model.SolReqPersonal = ObjSol;

                
                model.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                    && x.IdeSede == model.SolReqPersonal.IdeSede
                                                                    && x.IdeDependencia == model.SolReqPersonal.IdeDependencia
                                                                    ));


                model.listaDepartamento = new List<Departamento>(_departamentoRepository.GetBy(x => x.IdeDepartamento == model.SolReqPersonal.IdeDepartamento
                                                                                            && x.Dependencia.IdeDependencia == model.SolReqPersonal.IdeDependencia
                                                                                            && x.EstadoActivo == IndicadorActivo.Activo));


                model.listaArea = new List<Area>(_areaRepository.GetBy(x => x.IdeArea == model.SolReqPersonal.IdeArea
                                                                            && x.Departamento.IdeDepartamento == model.SolReqPersonal.IdeDepartamento
                                                                            && x.EstadoActivo == IndicadorActivo.Activo));

            }


            if (Etapa.Pendiente.Equals(EtapaSol))
            {
                if (Roles.Encargado_Seleccion.Equals(idRolUsuario))
                {
                    model.Accion = Accion.Aprobar;
                }    
            }

            if (Etapa.Aprobado.Equals(EtapaSol))
            {
                if (Roles.Encargado_Seleccion.Equals(idRolUsuario) || Roles.Analista_Seleccion.Equals(idRolUsuario))
                {
                    model.Accion = Accion.Publicar;
                }
            }

           

            

            return View("InformacionReemplazo", model);
        }


        /// <summary>
        /// Elimina el detalle del reemplazo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        public ActionResult EliminarReemplazo(int id, int idReq, int idPersona)
        {
            JsonMessage objJson = new JsonMessage();
            SolicitudRempCargoViewModel model;

            model = new SolicitudRempCargoViewModel();
           
            if (idReq>0)
            {
                Reemplazo objReemplazo = new Reemplazo();
                objReemplazo.IdeSolReqPersonal = idReq;
                objReemplazo.IdReemplazo = id;
                objReemplazo.IdPersona = idPersona;
                int dato = _solReqPersonalRepository.EliminaListaReemplazo(objReemplazo);
                
                if (dato==1)
	            {
	                objJson.Resultado=true;	 
                    objJson.Mensaje = "Se elimino el registro correctamente";
	            }
            
            }

            return Json(objJson);
        }

        /// <summary>
        /// lista de reemplazos
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListReemplazoReq(GridTable grid)
        {
            try
            {
                List<Reemplazo> ListaReemplazo = new List<Reemplazo>();
                Reemplazo ObjReemplazo;
              
                ObjReemplazo = new Reemplazo();


                if (grid.rules[0].data != null)
                {
                    var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == grid.rules[0].data);
                    ObjReemplazo.IdeSolReqPersonal = (int)objSol.IdeSolReqPersonal;
                }
                else
                {
                    ObjReemplazo.IdeSolReqPersonal = 0;
                }
               

                ListaReemplazo = _solReqPersonalRepository.GetListaReemplazo(ObjReemplazo);

                var generic = GetListar(ListaReemplazo,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);
               

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdReemplazo.ToString(),
                    cell = new string[]
                            {
                               
                                item.IdReemplazo==null?"":item.IdReemplazo.ToString(),
                                item.IdPersona==null?"":item.IdPersona.ToString(),
                                item.IdeSolReqPersonal==null?"":item.IdeSolReqPersonal.ToString(),
                                item.FecInicioReemplazo==null?"":String.Format("{0:MM/dd/yyyy}", item.FecInicioReemplazo), 
                                item.FecFinalReemplazo==null?"":String.Format("{0:MM/dd/yyyy}", item.FecFinalReemplazo),
                                item.Nombres==null?"":item.Nombres,
                                item.ApePaterno==null?"":item.ApePaterno,
                               
                               
                            }
                }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }
            

        /// <summary>
        /// Crea o Actuliza la Solicitud de reemplazo Requerimiento de personal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult SetSolReqPersonal(SolicitudRempCargoViewModel model) 
        {

            JsonMessage objJson = new JsonMessage();
            Int32 retorno=0;
            model.Reemplazo = new Reemplazo();

            try
            {
                model.SolReqPersonal.Feccreacion = FechaSistema;
                model.SolReqPersonal.UsuarioCreacion = UsuarioActual.NombreUsuario;
               
                var Sede = Session[ConstanteSesion.Sede];
                model.SolReqPersonal.IdeSede = Convert.ToInt32(Sede);

                var UsuarioSede = Session[ConstanteSesion.UsuarioSede];


                if (model.SolReqPersonal.CodSolReqPersonal!=null)
                {
                    var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == model.SolReqPersonal.CodSolReqPersonal);

                    objSol.IdeArea = model.SolReqPersonal.IdeArea;
                    objSol.IdeDependencia = model.SolReqPersonal.IdeDependencia;
                    objSol.IdeDepartamento = model.SolReqPersonal.IdeDepartamento;
                    objSol.Observacion = model.SolReqPersonal.Observacion;
                    objSol.TipPuesto = model.SolReqPersonal.TipPuesto;
                    objSol.TipVacante = model.SolReqPersonal.TipVacante;
                    objSol.NumVacantes = model.SolReqPersonal.NumVacantes;
                    objSol.FechaModificacion = FechaSistema;
                    objSol.UsuarioModificacion = UsuarioActual.NombreUsuario;
                    _solReqPersonalRepository.Update(objSol);

                    retorno = Convert.ToInt32(objSol.CodSolReqPersonal);
                    
                    if (retorno > 0)
                    {
                        objJson.Resultado = true;
                        objJson.Mensaje = "Se actualizo la Solicitud";
                        objJson.IdDato = retorno;
                    }

                }
                else
                {
                    //crea
                    if (UsuarioSede != null)
                    {
                        SedeNivel objSedeNivel = (SedeNivel)UsuarioSede;


                        model.SolReqPersonal.IdeArea = model.SolReqPersonal.IdeArea;
                        model.SolReqPersonal.IdeDependencia = model.SolReqPersonal.IdeDependencia;
                        model.SolReqPersonal.IdeDepartamento = model.SolReqPersonal.IdeDepartamento;
                        model.SolReqPersonal.idRolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                        model.SolReqPersonal.IdRolResp = 0;
                        model.SolReqPersonal.idUsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                        model.SolReqPersonal.idUsuarioResp = 0;
                        model.SolReqPersonal.Tipsol = TipoSolicitud.Ampliacion;

                        model.Reemplazo.CodGenerado = Convert.ToString(Session[ConstanteSesion.codReqSolTemp]);
                    }


                    model.SolReqPersonal.TipEtapa = Etapa.Pendiente;
                    retorno = _solReqPersonalRepository.CreaSolicitudReemplazo(model.SolReqPersonal, model.Reemplazo);

                    if (retorno > 0)
                    {
                        objJson.Resultado = true;
                        objJson.Mensaje = "Se genero la Solicitud";
                        objJson.IdDato = retorno;
                    }
                }
                

            }
            catch (Exception)
            {

                retorno = 0;
            }


            return Json(objJson);
        }

        /// <summary>
        /// Envia la solicitud de Reemplazo de requeremiento personal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EnviaSolReqPersonal(SolicitudRempCargoViewModel model) {

            JsonMessage objJson = new JsonMessage();
            int retorno=0;
           

            int idUsuario = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);

            if (model!=null)
            {

                if (model.SolReqPersonal.CodSolReqPersonal != null)
                {

                    var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == model.SolReqPersonal.CodSolReqPersonal);

                    if (objSol != null)
                    {
                        model.SolReqPersonal.IdeSolReqPersonal = objSol.IdeSolReqPersonal;
                        model.SolReqPersonal.idUsuarioSuceso = idUsuario;
                        model.SolReqPersonal.UsuarioCreacion = UsuarioActual.NombreUsuario;
                        model.SolReqPersonal.Feccreacion = FechaSistema;
                        model.SolReqPersonal.idRolSuceso = idRol;
                        model.SolReqPersonal.TipEtapa = Etapa.Pendiente;
                        model.SolReqPersonal.IdeCargo = objSol.IdeCargo;

                        var Sede = Convert.ToInt32(Session[ConstanteSesion.Sede]);

                        // Se obtiene el usaurio reponsable
                        var ObjUsuarioResp = _usuarioRolSedeRepository.GetBy(x => x.IdRol == Roles.Encargado_Seleccion
                                                            && x.IdSede == Sede
                                                            );
                        // se valida que exista y se toma al primer responsable
                        if (ObjUsuarioResp != null)
                        {
                            List<UsuarioRolSede> listaUsuarios = (List<UsuarioRolSede>)ObjUsuarioResp;
                            UsuarioRolSede usuarioRolSede = (UsuarioRolSede)listaUsuarios[0];
                            model.SolReqPersonal.idUsuarioResp = usuarioRolSede.IdUsuario;
                            model.SolReqPersonal.IdRolResp = usuarioRolSede.IdRol;
                            retorno = _solReqPersonalRepository.EnviaSolicitud(model.SolReqPersonal);
                        }
                        else
                        {
                            objJson.Resultado = false;
                            objJson.Mensaje = "No se encuentra un responsable(Encargado de selección) asignado";
                        }
                      
                    }
                    else
                    {
                        objJson.Resultado = false;
                        objJson.Mensaje = "no se puede enviar la solicitud, revise que los campos esten correctos";
                    }

                }
                else {
                    objJson.Resultado = false;
                    objJson.Mensaje = "Debe generar una solicitud";
                
                }
            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se puede enviar la solicitud, revise que los campos esten correctos";
            }

            if (retorno > 0)
            {
                objJson.Resultado = true;
                objJson.Mensaje = "Se envio la solicitud correctamente";
            }


            return Json(objJson);
        
        }

        /// <summary>
        /// inicializa el popup para las aprobaciones o rechazos.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult InicioPopupAprobReem(string id)
        {
            SolicitudRempCargoViewModel model;
            model = new SolicitudRempCargoViewModel();
         
            model.SolReqPersonal = new SolReqPersonal();
            model.LogSolReqPersonal = new LogSolReqPersonal();

            model.SolReqPersonal.CodSolReqPersonal = id;
            return View("PopupAprobacionRechazo", model);
        }

        /// <summary>
        /// obtiene los datos del popup de aprobacion y rechazo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetPopupAprobReem(SolicitudRempCargoViewModel model)
        {
            JsonMessage objJson = new JsonMessage();
            Boolean aprobacion = false;
            int retorno = 0;
            int sede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
            string tipoReq=null;
            SolReqPersonal objSolReqPersonal = null;


            if (model.SolReqPersonal.CodSolReqPersonal!=null)
            {
                aprobacion = model.LogSolReqPersonal.Aprobado;

                //se obtiene los datos de la solicitud
                var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == model.SolReqPersonal.CodSolReqPersonal);
                if (objSol!=null)
                {
                    model.LogSolReqPersonal.IdeSolReqPersonal = (int)objSol.IdeSolReqPersonal;
                    model.LogSolReqPersonal.UsrSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                    model.LogSolReqPersonal.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);

                    model.LogSolReqPersonal.FecSuceso = FechaSistema;
                    if (aprobacion)
                    {
                        model.LogSolReqPersonal.TipEtapa = Etapa.Aprobado;
                        model.LogSolReqPersonal.Observacion = "";

                        //se obtiene el tipo de requerimiento

                        var objCargo = _cargoRepository.GetSingle(x => x.IdeCargo == objSol.IdeCargo);
                        if (objCargo != null) 
                        {
                            tipoReq = objCargo.TipoRequerimiento;

                            objSolReqPersonal = new SolReqPersonal();
                            objSolReqPersonal = _solReqPersonalRepository.GetResponsable(TipoDerivacion.Publicado, sede, tipoReq);

                            model.LogSolReqPersonal.UsResponsable = objSolReqPersonal.idUsuarioResp;
                            model.LogSolReqPersonal.RolResponsable = objSolReqPersonal.IdRolResp;
                           

                            _solReqPersonalRepository.ActualizaLogSolReq(model.LogSolReqPersonal);

                            objSol.TipEtapa = Etapa.Aprobado;
                            objSol.FechaModificacion = FechaSistema;
                            objSol.UsuarioModificacion = UsuarioActual.NombreUsuario;
                            _solReqPersonalRepository.Update(objSol);

                            retorno = 1;

                        }
                        
                        if (retorno>0)
                        {
                            objJson.Resultado = true;
                            objJson.Mensaje = "Se Realizo la aprobación de la solicitud";
                        }
                    }
                    else
                    {
                        model.LogSolReqPersonal.TipEtapa = Etapa.Rechazado;

                        objSol.TipEtapa = Etapa.Rechazado;
                        objSol.FechaModificacion = FechaSistema;
                        objSol.UsuarioModificacion = UsuarioActual.NombreUsuario;

                        _solReqPersonalRepository.Update(objSol);

                        
                        _solReqPersonalRepository.ActualizaLogSolReq(model.LogSolReqPersonal);

                        if (retorno > 0)
                        {
                            objJson.Resultado = true;
                            objJson.Mensaje = "Se rechazo la solicitud";
                        }
                    }
                    
                }

            }

            if (retorno > 0)
            {
                objJson.Resultado = true;
                if (aprobacion)
                {
                    objJson.Mensaje = "Se aprobo la solicitud";
                }
                else
                {
                    objJson.Mensaje = "Se rechazo la solicitud";
                }
            }
          

            return Json(objJson);
            
        }

        [ValidarSesion]
        public ActionResult Publica(string id) 
        {
            SolicitudRempCargoViewModel model;
            model = new SolicitudRempCargoViewModel();
            model.SolReqPersonal = new SolReqPersonal();
            var ObjSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == id);

            if (ObjSol!=null)
            {
                model.SolReqPersonal.nombreCargo = ObjSol.nombreCargo;
                model.SolReqPersonal.DesCargo = ObjSol.DesCargo;
                model.SolReqPersonal.IdeSolReqPersonal = ObjSol.IdeSolReqPersonal;

                var objArea = _areaRepository.GetSingle(x => x.Departamento.IdeDepartamento == ObjSol.IdeDepartamento
                                                        && x.IdeArea == ObjSol.IdeArea);
                
                model.SolReqPersonal.Area_des = objArea.NombreArea;

                int TipoPuesto = Convert.ToInt32(TipoCampo.TipoSalario);

                var ObjDetalleGeneral = _detalleGeneralRepository.GetSingle(x => x.IdeGeneral == TipoPuesto 
                                                                            && x.Valor == ObjSol.TipPuesto);

                model.SolReqPersonal.TipPuestoDes = ObjDetalleGeneral.Descripcion==null?"":ObjDetalleGeneral.Descripcion;

                model.SolReqPersonal.NumVacantes = ObjSol.NumVacantes;
                model.SolReqPersonal.FuncionesCargo = ObjSol.FuncionesCargo;

                model.listaRangoSalarial =
                    new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
                model.listaRangoSalarial.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

                model.SolReqPersonal.TipoRangoSalario = ObjSol.TipoRangoSalario==null?"":ObjSol.TipoRangoSalario;
                
            }



            return View("Publicacion",model);
        }


        /// <summary>
        /// lista de conocimientos de la solicitud del requerimiento
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Conocimientos(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));

            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralRequerimiento>();
                where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                var generic = Listar(_ConocimientoGeneralRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionConocimientoGeneral,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }


        /// <summary>
        /// Estudios de la solicitud del requerimiento
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Estudios(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<NivelAcademicoRequerimiento>();
                where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                var generic = Listar(_nivelAcademicoRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionAreaEstudio,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// Compeetencias de la solictud del requerimiento
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Competencias(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<CompetenciaRequerimiento>();
                where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                var generic = Listar(_competenciaRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCompetenciaRequerimiento.ToString(),
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


        /// <summary>
        /// Lista de experiencias 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Experiencia(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            try
            {
                DetachedCriteria where = null;
                where = DetachedCriteria.For<ExperienciaRequerimiento>();

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;
  
                where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                var generic = Listar(_experienciaRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExperiencia,
                                item.CantidadAnhosExperiencia.ToString() + " AÑO(S)",
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// Lista de ofrecimientos
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Ofrecemos(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<OfrecemosRequerimiento>();
                where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                var generic = Listar(_ofrecemosRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeOfrecemosRequerimiento.ToString(),
                        cell = new string[]
                            {
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


        /// <summary>
        /// Realiza la publicacion de la solicitud de reemplazo de personal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult PublicaSolReqPersonal(SolicitudRempCargoViewModel model) 
        {
            JsonMessage objJson = new JsonMessage();
            var verSalario = model.verSalario;
            string IndVerSalario;

            if (verSalario)
            {
                IndVerSalario = "S";
            }else
	        {
                IndVerSalario = "N";
	        }


            if (model!=null)
            {

                var objSol = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(model.SolReqPersonal.IdeSolReqPersonal));
                if (objSol!=null)
                {

                    objSol.FecPublicacion = model.SolReqPersonal.FecPublicacion;
                    objSol.FechaModificacion = FechaSistema;
                    objSol.UsuarioModificacion = UsuarioActual.NombreUsuario;
                    objSol.FecExpiracacion = model.SolReqPersonal.FecExpiracacion;
                    objSol.TipEtapa = EtapasSolicitud.Publicado;
                    objSol.IndicadorSalario = IndVerSalario;
                    objSol.Observacion = model.SolReqPersonal.Observacion;

                    _solReqPersonalRepository.Update(objSol);

                    model.LogSolReqPersonal = new LogSolReqPersonal();
                    model.LogSolReqPersonal.IdeSolReqPersonal = (int)objSol.IdeSolReqPersonal;
                    model.LogSolReqPersonal.UsrSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                    model.LogSolReqPersonal.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                    model.LogSolReqPersonal.FecSuceso = FechaSistema;
                    model.LogSolReqPersonal.TipEtapa = EtapasSolicitud.Publicado;

                    _solReqPersonalRepository.ActualizaLogSolReq(model.LogSolReqPersonal);


                    objJson.Resultado = true;
                    objJson.Mensaje = "Se publico la Solicitud";
                }
            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se puede realizar la publicación de la solicitud";
            }



            return Json(objJson);
        }

        #endregion

        #region ranking
        public ActionResult PostulantesPorRequerimiento()
        {
            return View();
        }
        public ActionResult PostulantesPorRequerimientoCV()
        {
            return View();
        }
        public ActionResult PostulantesPreSeleccionados()
        {
            return View();
        }
        public ActionResult EvaluacionesPreSeleccionados()
        {
            return View();
        }

        public ActionResult ProgramarEvaluacion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListaPostulantesPorRequerimiento(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Linares Roca",
                    "Miguel Fransisco",
                    "",
                    "Pre-Selecc",
                    "",
                    "80"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Marsano Ramirez",
                    "Raul Ernesto",
                    "",
                    "Pre-Selecc",
                    "",
                    "70"
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Carmona Lino",
                    "Julio César",
                    "",
                    "No Pre-Selecc",
                    "",
                    "70"
                }
            };
            lstFilas.Add(fila3);

            var fila4 = new
            {
                id = 4,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Cevallos Tuesta",
                    "Yvette Isabel",
                    "",
                    "Excluido",
                    "No califica",
                    "70"
                }
            };
            lstFilas.Add(fila4);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }
        
        [HttpPost]

        public ActionResult ListaPostulantesPorRequerimientoCV(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Linares Roca",
                    "Miguel Fransisco",
                    "42158963",
                    "01-458796",
                    "28/11/13",
                    "10:25","",""
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Ochoa Ramirez",
                    "Maria ",
                    "42158963",
                    "01-458796",
                    "28/11/13",
                    "11:15","",""
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                     "Carbajal Saravia",
                    "Gianfranco ",
                    "42158963",
                    "01-458796",
                    "29/11/13",
                    "16:00","",""
                }
            };
            lstFilas.Add(fila3);

           //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }

        [HttpPost]
        public ActionResult ListaPostulantesPreSeleccionados(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Linares Roca",
                    "Miguel Fransisco",
                    "963452",
                    "94335624",
                    "",
                    "",
                    "Pre-Selecc",
                    "1/4",
                    "80"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Marsano Ramirez",
                    "Raul Ernesto",
                    "82452",
                    "94335624",
                    "",
                    "",
                    "Pre-Selecc",
                    "4/4",
                    "70"
                }
            };
            lstFilas.Add(fila2);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }
        
        [HttpPost]
        public ActionResult ListaEvaluacionesPreSeleccionados(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Examen 1",
                    "Examen",
                    "",
                    "20/05/2013",
                    "09:00 am.",
                    "RRHH",
                    "Evaluado",
                    "30"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Examen 2",
                    "Examen",
                    "",
                    "22/05/2013",
                    "03:00 pm.",
                    "RRHH",
                    "Programado",
                    "0"
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Entrevista",
                    "Entrevista",
                    "",
                    "",
                    "",
                    "",
                    "Pendiente",
                    "0"
                }
            };
            lstFilas.Add(fila3);

            var fila4 = new
            {
                id = 4,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Entrevista",
                    "Entrevista",
                    "",
                    "",
                    "",
                    "",
                    "Pendiente",
                    "0"
                }
            };
            lstFilas.Add(fila4);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }

        [HttpPost]
        public ActionResult ListaEvaluacionesDetalle(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                    "Lima",
                    "0",                    
                    "0"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                    "Arequipa",                    
                    "0",
                    "0"
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                    "Cusco",                    
                    "10",
                    "1"
                }
            };
            lstFilas.Add(fila3);

            var fila4 = new
            {
                id = 4,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                    "Lima",                    
                    "0",
                    "0"
                }
            };
            lstFilas.Add(fila4);
          

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }
        
        #endregion

        public ActionResult Publicacion()
        {
            return View();
        }

        #region ListaFunciones2
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaFunciones2(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila     
                  
                    "Coordinar con los Médicos",                    
                }
            };
            lstFilas.Add(fila1);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaCompetencias2
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaCompetencias2(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                   
                    "Responsabilidad",                    
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila   
                   
                    "Capacidad para resolver problemas",                    
                }
            };
            lstFilas.Add(fila2);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaOfrecemos2
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaOfrecemos2(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila       
                    
                    "Remuneración promedio al mercado",                    
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila    
                   
                    "Ingreso a planilla con todos los beneficios de ley",                    
                }
            };
            lstFilas.Add(fila2);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaHorarios2
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaHorarios2(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila       
                  
                    "Turno noche de 12 horas",                    
                }
            };
            lstFilas.Add(fila1);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaEstudiosPublicacion
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaEstudiosPublicacion(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila       
                    
                    "Técnico en Enfermería",                    
                }
            };
            lstFilas.Add(fila1);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaConocimientosPublicacion
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaConocimientosPublicacion(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila       
                    
                    "Microsoft Excel Avanzado",                    
                }
            };
            lstFilas.Add(fila1);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaExperienciaPublicacion
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaExperienciaPublicacion(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila       
                    
                    "Técnico en Enfermería",                    
                }
            };
            lstFilas.Add(fila1);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }





        #endregion

        public ActionResult ResultadoExamen1()
        {
            return View();
        }

        public ActionResult ResultadoExamen2()
        {
            return View();
        }

        public ActionResult ResultadoExamen3()
        {
            return View();
        }

        public ActionResult ResultadoExamen4()
        {
            return View();
        }

        public ActionResult ResultadoExamen5()
        {
            return View();
        }
    }
}
