namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;
    
    public class PostulanteController : Controller
    {
        private IPersonaRepository _personaRepository;
        private IEstudioPostulanteRepository _estudioPostulanteRepository;
        private IGeneralRepository _generalRepository;
        private IUbigeoRepository _ubigeoRepository;
        
        public PostulanteController(IPersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
                        
        }
        #region General
        public PostulanteGeneralViewModel inicializarPostulante()
        {
            var postulanteGeneralViewModel = new PostulanteGeneralViewModel();
            postulanteGeneralViewModel.Persona = new Persona();
            postulanteGeneralViewModel.TipoDocumentos = new List<ItemTabla>();
            postulanteGeneralViewModel.TipoDocumentos.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            postulanteGeneralViewModel.TipoDocumentos.Add(new ItemTabla { Codigo = "01", Descripcion = "DNI" });
            postulanteGeneralViewModel.TipoDocumentos.Add(new ItemTabla { Codigo = "02", Descripcion = "Carnet de Extranjeria" });

            postulanteGeneralViewModel.Nacionalidad = new List<ItemTabla>();
            postulanteGeneralViewModel.Nacionalidad.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            postulanteGeneralViewModel.Nacionalidad.Add(new ItemTabla { Codigo = "01", Descripcion = "Peruana" });
            postulanteGeneralViewModel.Nacionalidad.Add(new ItemTabla { Codigo = "02", Descripcion = "Chilena" });

            postulanteGeneralViewModel.Sexo = new List<ItemTabla>();
            postulanteGeneralViewModel.Sexo.Add(new ItemTabla { Codigo = "0", Descripcion = "Seleccionar" });
            postulanteGeneralViewModel.Sexo.Add(new ItemTabla { Codigo = "1", Descripcion = "Masculino" });
            postulanteGeneralViewModel.Sexo.Add(new ItemTabla { Codigo = "2", Descripcion = "Femenino" });

            postulanteGeneralViewModel.EstadosCiviles = new List<ItemTabla>();
            postulanteGeneralViewModel.EstadosCiviles.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            postulanteGeneralViewModel.EstadosCiviles.Add(new ItemTabla { Codigo = "01", Descripcion = "Soltero(a)" });
            postulanteGeneralViewModel.EstadosCiviles.Add(new ItemTabla { Codigo = "02", Descripcion = "Casado(a)" });
            postulanteGeneralViewModel.EstadosCiviles.Add(new ItemTabla { Codigo = "02", Descripcion = "Viudo(a)" });
            postulanteGeneralViewModel.EstadosCiviles.Add(new ItemTabla { Codigo = "02", Descripcion = "Divorciado(a)" });

            postulanteGeneralViewModel.Departamentos = new List<ItemTabla>();
            postulanteGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "0", Descripcion = "Seleccionar" });
            postulanteGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "1", Descripcion = "Amazonas" });
            postulanteGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "2", Descripcion = "Ancash" });
            postulanteGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "3", Descripcion = "Apurimac" });
            postulanteGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "4", Descripcion = "Arequipa" });
            postulanteGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "5", Descripcion = "Cusco" });
            postulanteGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "6", Descripcion = "Lima" });
            postulanteGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "7", Descripcion = "Loreto" });
            postulanteGeneralViewModel.Departamentos.Add(new ItemTabla { Codigo = "8", Descripcion = "Madre de Dios" });


            /*var departamentos = new List<ItemTabla>();
            postulanteGeneralViewModel.Departamentos = new List<ItemTabla>();
            var obtenerDepartamentos = _ubigeoRepository.GetBy(x => x.Codigo == "010000");
            foreach (var datos in obtenerDepartamentos)
            {
                departamentos.Add(new ItemTabla
                {
                    Codigo = datos.IdeUbigeo.ToString(),
                    Descripcion = datos.Nombre
                });
            }
            postulanteGeneralViewModel.Departamentos = departamentos;*/
            
            return postulanteGeneralViewModel;
        }
        public ViewResult General()
        {
            var postulanteGeneralViewModel = inicializarPostulante();
            return View(postulanteGeneralViewModel);
        }

        [HttpPost]
        public ActionResult General([Bind(Prefix = "Persona")]Persona persona)
        {
                       
            if (!ModelState.IsValid)
            {
                var personaModel = inicializarPostulante();
                personaModel.Persona = persona;
                return View("General",personaModel);
            }
            _personaRepository.Add(persona);
            return RedirectToAction("Estudios");
        }
        #endregion


        #region Estudios
        public EstudioPostulanteGeneralViewModel inicializarEstudio()
        {
            var estudioPostulanteGeneralViewModel = new EstudioPostulanteGeneralViewModel();
            estudioPostulanteGeneralViewModel.Estudio = new EstudioPostulante();
            estudioPostulanteGeneralViewModel.TipoInstituciones = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "01", Descripcion = "Universidad" });
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "02", Descripcion = "Instituto" });
            estudioPostulanteGeneralViewModel.TipoInstituciones.Add(new ItemTabla { Codigo = "03", Descripcion = "Colegio" });

            var listaInstituciones = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.Instituciones = new List<ItemTabla>();
            var recuperarInstituciones = _personaRepository.GetBy(x => x.PrimerNombre == "LIKE '%J'" );
            foreach (var data in recuperarInstituciones)
            { 
                listaInstituciones.Add(new ItemTabla
                {
                    Codigo = data.TipoDocumento,
                    Descripcion = data.ApellidoPaterno
                });
            }
            estudioPostulanteGeneralViewModel.Instituciones = listaInstituciones;
            estudioPostulanteGeneralViewModel.Instituciones.Insert(0, new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });

            estudioPostulanteGeneralViewModel.AreasEstudio = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "01", Descripcion = "Medicina General" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "02", Descripcion = "Ing. Informática y de Sistemas" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "03", Descripcion = "Enfemeria" });
            estudioPostulanteGeneralViewModel.AreasEstudio.Add(new ItemTabla { Codigo = "04", Descripcion = "Medicina Pediatrica" });

            estudioPostulanteGeneralViewModel.Educacion = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "01", Descripcion = "Secundaria" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "02", Descripcion = "Tecnica" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "03", Descripcion = "Universitaria" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "04", Descripcion = "Maestria" });
            estudioPostulanteGeneralViewModel.Educacion.Add(new ItemTabla { Codigo = "05", Descripcion = "Doctorado" });

            estudioPostulanteGeneralViewModel.NivelesAlcanzados = new List<ItemTabla>();
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "00", Descripcion = "Seleccionar" });
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "01", Descripcion = "Incompleta" });
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "02", Descripcion = "Completa" });
            estudioPostulanteGeneralViewModel.NivelesAlcanzados.Add(new ItemTabla { Codigo = "03", Descripcion = "En curso" });
            
            return estudioPostulanteGeneralViewModel;
        }
        

        public ViewResult Estudios()
        {
            var estudioGeneralViewModel = inicializarEstudio();
            return View(estudioGeneralViewModel);
        }

        [HttpPost]
        public ActionResult Estudios([Bind(Prefix = "Estudio")]EstudioPostulante estudioPostulante)
        {

            if (!ModelState.IsValid)
            {
                var estudioPostulanteModel = inicializarEstudio();
                estudioPostulanteModel.Estudio = estudioPostulante;
                return View("Estudios", estudioPostulanteModel);
            }
            
            _estudioPostulanteRepository.Add(estudioPostulante);
            return RedirectToAction("Experiencia");
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

        public ActionResult DatosComplementarios()
        {
            return View("DatosComplementarios");
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

        #region ListaEstudios
        /// <summary>
        /// Lista de Estudios
        /// </summary>       
        [HttpPost]
        public ActionResult ListaEstudios(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Universidad",
                          "Universidad de Lima",
                          "Administración",                          
                          "Pregrado",
                          "Completa",
                          "Ene/2005",
                          "Dic/2010",
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "Colegio",
                          "Colegio la Recoleta",
                          "",                          
                          "Secundaria",
                          "Completa",
                          "Ene/2008",
                          "Dic/2012",                    
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
