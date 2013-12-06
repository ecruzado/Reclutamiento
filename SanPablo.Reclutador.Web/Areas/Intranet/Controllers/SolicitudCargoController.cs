using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    public class SolicitudCargoController : Controller
    {

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
            var fAnonymousType2_1 = new
            {
                id = 1,
                cell = new string[19]
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
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_1);
            var fAnonymousType2_2 = new
            {
                id = 2,
                cell = new string[19]
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
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_2);
            var fAnonymousType2_3 = new
            {
                id = 3,
                cell = new string[19]
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
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_3);
            var fAnonymousType2_4 = new
            {
                id = 4,
                cell = new string[19]
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
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_4);
            var fAnonymousType2_5 = new
            {
                id = 5,
                cell = new string[19]
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
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_5);
            var fAnonymousType2_6 = new
            {
                id = 6,
                cell = new string[19]
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
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_6);
            var fAnonymousType2_7 = new
            {
                id = 7,
                cell = new string[19]
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
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_7);
            var fAnonymousType2_8 = new
            {
                id = 8,
                cell = new string[19]
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
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_8);
            var fAnonymousType2_9 = new
            {
                id = 9,
                cell = new string[19]
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
          "Activo"
        }
            };
            list.Add((object)fAnonymousType2_9);
            var fAnonymousType2_10 = new
            {
                id = 10,
                cell = new string[19]
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

        public ActionResult ListaReemplazo()
        {
            return View();
        }

        public ActionResult CrearReemplazo()
        {
            return View("InformacionReemplazo");
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
