﻿namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web;
    using System.Web.Services;
    using SanPablo.Reclutador.Entity.Validation;
    using FluentValidation.Results;
    using FluentValidation;
    
    public class PostulanteController : Controller
    {
        private IPostulanteRepository _postulanteRepository;
        private IEstudioPostulanteRepository _estudioPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IUbigeoRepository _ubigeoRepository;
        private PostulanteGeneralViewModel postulanteModel = new PostulanteGeneralViewModel();
                
        public PostulanteController(IPostulanteRepository postulanteRepository,IEstudioPostulanteRepository estudioPostulanteRepository,IUbigeoRepository ubigeoRepository, IDetalleGeneralRepository detalleGeneralRepository)
        {
            _postulanteRepository = postulanteRepository;
            _estudioPostulanteRepository = estudioPostulanteRepository;
            _ubigeoRepository = ubigeoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
                        
        }
        #region General
        public PostulanteGeneralViewModel inicializarPostulante()
        {
            var postulanteGeneralViewModel = new PostulanteGeneralViewModel();
            postulanteGeneralViewModel.Postulante = new Postulante();
            postulanteGeneralViewModel.TipoDocumentos = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoDocumento));
            postulanteGeneralViewModel.TipoDocumentos.Insert(0,new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            
            postulanteGeneralViewModel.Nacionalidad = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.Nacionalidad));
            postulanteGeneralViewModel.Nacionalidad.Insert(0,new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            
            postulanteGeneralViewModel.Sexo = new List<DetalleGeneral>( _detalleGeneralRepository.GetByTipoTabla(TipoTabla.Sexo));
            postulanteGeneralViewModel.Sexo.Insert(0,new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            
            postulanteGeneralViewModel.EstadosCiviles = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoCivil));
            postulanteGeneralViewModel.EstadosCiviles.Insert(0,new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            
            postulanteGeneralViewModel.TipoVias = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoVia));
            postulanteGeneralViewModel.TipoVias.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoZonas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoZona));
            postulanteGeneralViewModel.TipoZonas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.Departamentos = new List<Ubigeo>();
            postulanteGeneralViewModel.Departamentos = cargarDepartamentos();
            postulanteGeneralViewModel.Departamentos.Insert(0, new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            postulanteGeneralViewModel.Provincias = new List<Ubigeo>();
            postulanteGeneralViewModel.Provincias.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            postulanteGeneralViewModel.Distritos = new List<Ubigeo>();
            postulanteGeneralViewModel.Distritos.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });
                      
            return postulanteGeneralViewModel;
        }
        public ViewResult General()
        {
            var postulanteGeneralViewModel = inicializarPostulante();
            return View(postulanteGeneralViewModel);
        }

        [HttpPost]
        public ActionResult General([Bind(Prefix = "Postulante")]Postulante postulante)
        {
            PostulanteValidator validator = new PostulanteValidator();
            ValidationResult result = validator.Validate(postulante, "TipoDocumento", "NumeroDocumento", "ApellidoPaterno", "ApellidoMaterno", "PrimerNombre",
                                      "SegundoNombre", "FechaNacimiento", "IndicadorSexo", "TipoEstadoCivil", "IdeUbigeo", "Correo", "TipoVia", "NumeroDireccion");

            if (!result.IsValid)
            {
                postulanteModel = inicializarPostulante();
                postulanteModel.Postulante = postulante;
                return View("General",postulanteModel);
            }
            _postulanteRepository.Add(postulante);
            return RedirectToAction("EstudioPostulante/Index");
        }
        #endregion


        #region DatosComplementarios

        public PostulanteGeneralViewModel inicializarDatosComplementarios()
        {
            var postulanteGeneralViewModel = new PostulanteGeneralViewModel();
            postulanteGeneralViewModel.Postulante = new Postulante();
            postulanteGeneralViewModel.TipoSueldosBrutos = _detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario);
            postulanteGeneralViewModel.TipoSueldosBrutos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoDisponibilidadesTrabajos = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.DisponibilidadTrabajo));
            postulanteGeneralViewModel.TipoDisponibilidadesTrabajos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoDisponibilidadesHorarios = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.DisponibilidadHorario));
            postulanteGeneralViewModel.TipoDisponibilidadesHorarios.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoHorarios = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            postulanteGeneralViewModel.TipoHorarios.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoParientesSedes = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoParienteSede));
            postulanteGeneralViewModel.TipoParientesSedes.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return postulanteGeneralViewModel;
        }
        public ViewResult DatosComplementarios()
        {
            var postulanteGeneralViewModel = inicializarDatosComplementarios();
            return View(postulanteGeneralViewModel);
        }

        [HttpPost]
        public ActionResult DatosComplementarios([Bind(Prefix = "Postulante")]Postulante postulante)
        {
            PostulanteValidator validator = new PostulanteValidator();
            ValidationResult result = validator.Validate(postulante, "TipoSalario", "TipoDisponibilidadTrabajo", "TipoDisponibilidadHorario", "TipoComoSeEntero");
            
            if (!result.IsValid)
            {
                postulanteModel = inicializarDatosComplementarios();
                postulanteModel.Postulante = postulante;
                return View("DatosComplementarios", postulanteModel);
            }
            _postulanteRepository.Update(postulante);
            return RedirectToAction("Parientes");
        }

        #endregion

        #region METODOS

        public List<Ubigeo> cargarDepartamentos()
        {
            var departamentos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == null));
            return departamentos;
            
        }
        [HttpPost]
        public ActionResult  listarUbigeos (int ideUbigeoPadre)
        {
             ActionResult result = null;

             var listaResultado = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == ideUbigeoPadre));
            result = Json(listaResultado);
            return result;
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Conocimientos()
        {
            return View("Conocimientos");
        }

        public ActionResult DiscapacidadOtros()
        {
            return View("DiscapacidadOtros");
        }

        public ActionResult ExperienciaLaboral()
        {
            return View("ExperienciaLaboral");
        }

        public ActionResult Parientes()
        {
            return View("Parientes");
        }

        public ActionResult Referencias()
        {
            return View("Referencias");
        }

        public ActionResult Reclutamientos()
        {
            return View("Reclutamientos");
        }

        public ActionResult DetalleCargo()
        {
            return View("DetalleCargo");
        }

        public ActionResult InstruccionesExamen()
        {
            return View("InstruccionesExamen");
        }

        public ActionResult Lista()
        {
            return View();
        }

        public ActionResult Postulaciones()
        {
            return View();
        }
        public ActionResult EvaluacionPostulante()
        {
            return View("EvaluacionPostulante");
        }

        public ActionResult Examen()
        {
            return View("Examen");
        }


        #region ListaMisPostulaciones
        /// <summary>
        /// Lista de Mis Postulaciones
        /// </summary>       
        [HttpPost]
        public ActionResult ListaMisPostulaciones(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Jefe de Admisión de Emergencia",
                          "Lima",
                          "05/02/2013",
                          "05/06/2013",                         
                          "Vigente",
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

        #region ListaOfertasLaborales
        /// <summary>
        /// Lista de Ofertas Laborales
        /// </summary>       
        [HttpPost]
        public ActionResult ListaOfertasLaborales(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "13/05/2013",
                          "Santiago de Surco, Lima",
                          "Secretaria de Hospitalización",                          
                          "Tiempo Parcial",    
                          "15/07/2013",
                          "Hospitalización",
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "23/05/2013",
                          "San Juan de Miraflores, Lima",
                          "Técnico en Enfermería",                          
                          "Tiempo Completo",            
                          "15/07/2013",
                          "Hospitalización"
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "23/05/2013",
                          "Rimac, Lima",
                          "Ayudante de cocina",                          
                          "Tiempo Completo",         
                          "31/07/2013",
                          "Cocina"
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
        #endregion

        #region ListaReferencias
        /// <summary>
        /// Lista de Referencias
        /// </summary>       
        [HttpPost]
        public ActionResult ListaReferencias(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "1",
                          "Clinica San Pedro",
                          "Juan Ramon Noriega Vargas",
                          "Banco Scotibank",
                          "619-8964",
                          "4080",                         
                          "jnoriega@gmail.com",                         
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "2",
                          "Minsa",
                          "Juan Peréz",
                          "Banco BCP",
                          "586-4128",
                          "2385",                         
                          "jperez@gmail.com",                         
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

        #region ListaParientes
        /// <summary>
        /// Lista de Parientes
        /// </summary>       
        [HttpPost]
        public ActionResult ListaParientes(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Linares",
                          "Rodriguez",
                          "Juan Carlos",                          
                          "Padre","",
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Roca",
                          "Tello",
                          "Rosa María",                          
                          "Madre", "",                     
                }
            };
            lstFilas.Add(fila2);
            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Valverde",
                          "Bravo",
                          "Hanna",                          
                          "Hija", "07/06/2012",                     
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
        #endregion

        #region ListaExperienciaLaboral
        /// <summary>
        /// Lista de Experiencia Laboral
        /// </summary>       
        [HttpPost]
        public ActionResult ListaExperienciaLaboral(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Clinica Ricardo Palma",
                          "Especialista en RX",                          
                          "Ene/2010",
                          "Dic/2011",
                          "2 años",
                          "Despido",
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Clinica Internacional",
                          "Asist. Enfermería ",                          
                          "Ene/2008",
                          "Dic/2009",                    
                          "2 años",
                          "Renuncia",
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

        #region ListaFuncionesDesempenhadas
        /// <summary>
        /// Lista de Funciones desempeñadas
        /// </summary>       
        [HttpPost]
        public ActionResult ListaFuncionesDesempenhadas(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                     "0045",
                          "0045",
                          "Control de Calidad",                                                    
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                     "0046",
                          "0046",
                          "Capacitación de personal",                                             
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

        #region ListaConocimientosGenerales
        /// <summary>
        /// Lista de Conocimientos Generales
        /// </summary>       
        [HttpPost]
        public ActionResult ListaConocimientosGenerales(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "52456",
                          "Software",                          
                          "Microsoft Word",
                          "Avanzado",
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "695521",
                          "Software",                          
                          "Microsoft Excel",
                          "Intermedio",                    
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

        #region ListaConocimientosIdiomas
        /// <summary>
        /// Lista de Conocimientos de idiomas
        /// </summary>       
        [HttpPost]
        public ActionResult ListaConocimientosIdiomas(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "08778",
                          "Inglés",                          
                          "Escritura",
                          "Avanzado",
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "08779",
                          "Inglés",                          
                          "Lectura",
                          "Básico",                    
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

        #region ListaDiscapacidad
        /// <summary>
        /// Lista de Discapacidad
        /// </summary>       
        [HttpPost]
        public ActionResult ListaDiscapacidad(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Motora",                          
                          "Cojera pierna izquierda",                          
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

        #region ListaExamenesPendientes
        /// <summary>
        /// Lista de Discapacidad
        /// </summary>       
        [HttpPost]
        public ActionResult ListaExamenesPendientes(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Proactividad",
                          "10 minutos",                          
                          "Evaluado",
                          "0"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Comunicación Interpersonal",                                                    
                          "30 minutos",                          
                          "Pendiente",
                          "1"
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


        #region ListaReclutamientosPotulante
        /// <summary>
        /// Lista de Discapacidad
        /// </summary>       
        [HttpPost]
        public ActionResult ListaReclutamientosPotulante(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "RE0001",
                          "Técnico en enfermería",
                          "50",                          
                          "0",
                          "REG",
                          "23/01/2013"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "RE0017",
                          "Asistente",                                                    
                          "0",                          
                          "0",
                          "REG",
                          "23/01/2013"
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

        #region ListaEvaluacionesxReclutamiento
        /// <summary>
        /// Lista de Discapacidad
        /// </summary>       
        [HttpPost]
        public ActionResult ListaEvaluacionesxReclutamiento(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "EV00012",
                          "Examen 01",
                          "Examen",                          
                          "50",
                          
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "EV00013",
                          "Examen 02",                                                    
                          "Entrevista",                          
                          "0",                         
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

    }
}
