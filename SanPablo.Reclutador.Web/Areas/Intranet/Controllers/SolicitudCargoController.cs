

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
      
        public SolicitudCargoController( IDetalleGeneralRepository detalleGeneralRepository,
                                         ISolReqPersonalRepository solReqPersonalRepository,
                                         IDependenciaRepository dependenciaRepository,
                                         IAreaRepository areaRepository,
                                         IDepartamentoRepository departamentoRepository,
                                         IUsuarioRolSedeRepository usuarioRolSedeRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _dependenciaRepository = dependenciaRepository;
            _areaRepository = areaRepository;
            _departamentoRepository = departamentoRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            
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
               

                if ((!"".Equals(grid.rules[1].data) && !"0".Equals(grid.rules[1].data) && !0.Equals(grid.rules[1].data)) ||
                    (!"".Equals(grid.rules[2].data) && !"0".Equals(grid.rules[2].data) && !0.Equals(grid.rules[2].data)) ||
                    (!"".Equals(grid.rules[3].data) && !"0".Equals(grid.rules[3].data) && !0.Equals(grid.rules[3].data)) ||
                    (!"".Equals(grid.rules[4].data) && !"0".Equals(grid.rules[4].data) && !0.Equals(grid.rules[4].data)) ||
                    (!"".Equals(grid.rules[5].data) && grid.rules[5].data != null ) ||
                    (!"".Equals(grid.rules[6].data) && grid.rules[6].data != null ) ||
                    (!"".Equals(grid.rules[7].data) && !"0".Equals(grid.rules[7].data) && !0.Equals(grid.rules[7].data)) ||
                    (!"".Equals(grid.rules[8].data) && grid.rules[8].data != null && !"0".Equals(grid.rules[8].data)) ||
                    (!"".Equals(grid.rules[9].data) && grid.rules[9].data != null && !"0".Equals(grid.rules[9].data))
                    
                    )
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
                }


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
                               
                                item.IdRol==null?"":item.IdRol.ToString(),
                                item.DesRol==null?"":item.DesRol,
                                item.NomPersonReemplazo==null?"":item.NomPersonReemplazo,
                                
                                item.FlagPublicado==null?"":item.FlagPublicado,
                                item.TipEtapa==null?"":item.TipEtapa
                               
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
            }
            
            model.SolReqPersonal = new SolReqPersonal();

            model.Accion = Accion.Nuevo;
            var codUsuario = Convert.ToString(Session[ConstanteSesion.Usuario]);
            var codSede = Convert.ToString(Session[ConstanteSesion.Sede]);
            var codRol = Convert.ToString(Session[ConstanteSesion.Rol]);

            CodGenerado = codUsuario+codSede+codRol+String.Format("{0:MM/dd/yyyy}", hoy) + System.Guid.NewGuid().ToString();
            codCodificado = Base64Encode(CodGenerado);

            Session[ConstanteSesion.codReqSolTemp] = codCodificado;


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
                                                                        && x.IdeSede == UsuarioSede.IDESEDE
                                                                        && x.IdeDependencia ==UsuarioSede.IDEDEPENDENCIA
                                                                        ));
            

            objModel.listaDepartamento = new List<Departamento>(_departamentoRepository.GetBy(x => x.IdeDepartamento == UsuarioSede.IDEDEPARTAMENTO 
                                                                                              && x.Dependencia.IdeDependencia == UsuarioSede.IDEDEPENDENCIA          
                                                                                              && x.EstadoActivo == IndicadorActivo.Activo));
            

            objModel.listaArea = new List<Area>(_areaRepository.GetBy(x => x.IdeArea == UsuarioSede.IDEAREA
                                                                      && x.Departamento.IdeDepartamento == UsuarioSede.IDEDEPARTAMENTO
                                                                      && x.EstadoActivo == IndicadorActivo.Activo));
            


            return objModel;
        }

       /// <summary>
       /// Incializa el popup de reemplazo
       /// </summary>
       /// <returns></returns>
        public ActionResult InicioPopupReemplazo() 
        {

            SolicitudRempCargoViewModel model;

            model = new SolicitudRempCargoViewModel();

            model.Reemplazo = new Reemplazo();

            DateTime Hoy = DateTime.Today;
            
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
            string codGeneredo =null;
            int dato = 0;
            codGeneredo = Convert.ToString(Session[ConstanteSesion.codReqSolTemp]);
            List<Reemplazo> lista = new List<Reemplazo>();
           
            var listaInicio = Session[ConstanteSesion.ListaReemplazo];

            if (listaInicio!=null)
            {
                List<Reemplazo> list = (List<Reemplazo>)listaInicio;
                foreach (Reemplazo item in list)
                {
                   
                    objReemplazo = new Reemplazo();
                    objReemplazo = item;
                    lista.Add(objReemplazo);
  
                }

                objReemplazo = new Reemplazo();
                objReemplazo.Indicador = lista.Count + 1;

                objReemplazo.ApePaterno = model.Reemplazo.ApePaterno;
                objReemplazo.ApeMaterno = model.Reemplazo.ApeMaterno;
                objReemplazo.FecInicioReemplazo = model.Reemplazo.FecInicioReemplazo;
                objReemplazo.FecFinalReemplazo = model.Reemplazo.FecFinalReemplazo;
                objReemplazo.Nombres = model.Reemplazo.ApeMaterno;

                lista.Add(objReemplazo);

                objReemplazo.CodGenerado = codGeneredo;
                objReemplazo.FechaCreacion = FechaSistema;
                objReemplazo.UsuarioCreacion = UsuarioActual.NombreUsuario;
                dato = _solReqPersonalRepository.InsertTempReemplazo(objReemplazo);

                Session[ConstanteSesion.ListaReemplazo] = lista;
                objJson.Mensaje = "Se grabo el registro correctamente";
                objJson.Resultado = true;

            }
            else
            {
                objReemplazo = new Reemplazo();
                objReemplazo.Indicador = objReemplazo.Indicador + 1;

                objReemplazo.ApePaterno = model.Reemplazo.ApePaterno;
                objReemplazo.ApeMaterno = model.Reemplazo.ApeMaterno;
                objReemplazo.FecInicioReemplazo = model.Reemplazo.FecInicioReemplazo;
                objReemplazo.FecFinalReemplazo = model.Reemplazo.FecFinalReemplazo;
                objReemplazo.Nombres = model.Reemplazo.ApeMaterno;

                lista.Add(objReemplazo);

                objReemplazo.CodGenerado = codGeneredo;
                objReemplazo.FechaCreacion = FechaSistema;
                objReemplazo.UsuarioCreacion = UsuarioActual.NombreUsuario;
                dato = _solReqPersonalRepository.InsertTempReemplazo(objReemplazo);


                Session[ConstanteSesion.ListaReemplazo] = lista;
                objJson.Mensaje = "Se grabo el registro correctamente";
                objJson.Resultado = true;
            }

            return Json(objJson);
        }

     

        /// <summary>
        /// Elimina el detalle del reemplazo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        
        public ActionResult EliminarReemplazo(int id,int idReq,int idReemp)
        {
            JsonMessage objJson = new JsonMessage();
            SolicitudRempCargoViewModel model;

            model = new SolicitudRempCargoViewModel();
            var ListaSession = Session[ConstanteSesion.ListaReemplazo];

            if (ListaSession!=null)
            {
                List<Reemplazo> lista = (List<Reemplazo>)ListaSession;
                if (id>0)
                {
                    id = id - 1;
                }
                else
                {
                    id = 0;
                }
                lista.RemoveAt(id);
                Session[ConstanteSesion.ListaReemplazo] = lista;

                objJson.Resultado=true;	 
                objJson.Mensaje = "Se elimino el registro correctamente";

            }

            if (idReq>0 && idReemp>0)
            {
                Reemplazo objReemplazo = new Reemplazo();
                objReemplazo.IdeSolReqPersonal = idReq;
                objReemplazo.IdReemplazo = idReemp;
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
                var objListaReemplazo = Session[ConstanteSesion.ListaReemplazo];



                if ((!"".Equals(grid.rules[0].data) && !"0".Equals(grid.rules[0].data)))
                {
                    if (objListaReemplazo != null)
                    {

                        List<Reemplazo> lista = (List<Reemplazo>)objListaReemplazo;

                        ListaReemplazo =
                                    (lista.Where(n => n.IdeSolReqPersonal == Convert.ToInt32(grid.rules[0].data)).ToList());
                        
                        
                    }
                    else
                    {
                        ObjReemplazo = new Reemplazo();
                        ObjReemplazo.IdeSolReqPersonal = Convert.ToInt32(grid.rules[0].data);
                        ListaReemplazo = _solReqPersonalRepository.GetListaReemplazo(ObjReemplazo);

                    }

                }
                else {

                    if (objListaReemplazo != null)
                    {

                        List<Reemplazo> lista = (List<Reemplazo>)objListaReemplazo;

                        ListaReemplazo = lista;

                    }
                    else 
                    { 
                        ObjReemplazo = new Reemplazo();
                      
                        ListaReemplazo.Add(ObjReemplazo);
                        return null;
                    }
                
                }

                Session[ConstanteSesion.cantRegListaReemplazo] = ListaReemplazo.Count();

               

                var generic = GetListar(ListaReemplazo,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);
               

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.Indicador.ToString(),
                    cell = new string[]
                            {
                               
                                item.Indicador==null?"0":item.Indicador.ToString(),
                                item.IdeSolReqPersonal==null?"":item.IdeSolReqPersonal.ToString(),
                                item.IdReemplazo==null?"":item.IdReemplazo.ToString(),
                                item.FecInicioReemplazo==null?"":String.Format("{0:MM/dd/yyyy}", item.FecInicioReemplazo), 
                                item.FecFinalReemplazo==null?"":String.Format("{0:MM/dd/yyyy}", item.FecFinalReemplazo),
                                item.Nombres==null?"":item.Nombres,
                                item.ApePaterno==null?"":item.ApePaterno,
                                item.ApeMaterno==null?"":item.ApeMaterno
                               
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
        /// Crea Solicitud de reemplazo Requerimiento de personal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult SetSolReqPersonal(SolicitudRempCargoViewModel model) 
        {

            JsonMessage objJson = new JsonMessage();
            string retorno=null;
            model.Reemplazo = new Reemplazo();

            try
            {
                model.SolReqPersonal.Feccreacion = FechaSistema;
                model.SolReqPersonal.UsuarioCreacion = UsuarioActual.NombreUsuario;
                int codDepartamento;
                int codArea;
                int codDependencia;
                var Sede = Session[ConstanteSesion.Sede];
                model.SolReqPersonal.IdeSede = Convert.ToInt32(Sede);

                var UsuarioSede = Session[ConstanteSesion.UsuarioSede];

                if (UsuarioSede!=null)
                {
                    SedeNivel objSedeNivel = (SedeNivel)UsuarioSede;
                    codDepartamento = objSedeNivel.IDEDEPARTAMENTO;
                    codArea = objSedeNivel.IDEAREA;
                    codDependencia = objSedeNivel.IDEDEPENDENCIA;

                    model.SolReqPersonal.IdeArea = codArea;
                    model.SolReqPersonal.IdeDependencia = codDependencia;
                    model.SolReqPersonal.IdeDepartamento = codDepartamento;

                    model.Reemplazo.CodGenerado = Convert.ToString(Session[ConstanteSesion.codReqSolTemp]);
                }
                
                

                retorno = _solReqPersonalRepository.CreaSolicitudReemplazo(model.SolReqPersonal,model.Reemplazo);

            }
            catch (Exception)
            {

                retorno = "";
            }


            if (retorno!="")
            {
                objJson.Resultado = true;
                objJson.Mensaje = "Se genero la Solicitud";
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
