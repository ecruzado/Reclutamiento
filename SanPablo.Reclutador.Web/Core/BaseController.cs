using SanPablo.Reclutador.Web.Models;
using SanPablo.Reclutador.Web.Models.JQGrid;
using SanPablo.Reclutador.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NHibernate.Criterion;
using SanPablo.Reclutador.Entity;


namespace SanPablo.Reclutador.Web.Core
{
    public class BaseController : Controller
    {
        #region Paginacion

        protected GenericDouble<JQgrid, T> Listar<T>(IRepository<T> logic, string sidx, string sord, int pageIndex, int pageSize, bool search, string searchField, string searchOper, string searchString) where T : class
        {
            return Listar(logic, sidx, sord, pageIndex, pageSize, search, searchField, searchOper, searchString, null);
        }
        
        protected GenericDouble<JQgrid, T> Listar<T>(IRepository<T> logic, string sidx, string sord, int pageIndex, int pageSize, bool search, string searchField, string searchOper, string searchString, DetachedCriteria where) where T : class
        {
            Boolean orden = false;
            
            if (string.IsNullOrEmpty(sidx))
            {
                sidx = string.Empty;
            }
            if (string.IsNullOrEmpty(sord))
            {
                sord = "desc";
            }

            if ("desc".Equals(sord))
            {
                orden = false;
            }
            else
            {
                orden = true;
            }

            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 100;
            }

            var jqgrid = new JQgrid();
            IList<T> list;

            try
            {
                int totalPages = 0;

                //if (search)
                //{
                //    @where = @where + GetWhere(searchField, searchOper, searchString);
                //}

                //var count = logic.CountBy();
                var count = 0;
                if (where != null)
                    count = logic.CountBy(where);
                else
                    count = logic.CountBy();


                if (count > 0 && pageSize > 0)
                {
                    if (count % pageSize > 0)
                    {
                        totalPages = (int)Math.Ceiling((decimal)(count / pageSize));
                        totalPages = (int)(count / pageSize) + 1;
                    }
                    else
                    {
                        totalPages = (int)(count / pageSize);
                    }
                    totalPages = totalPages == 0 ? 1 : totalPages;
                }

                pageIndex = pageIndex > totalPages ? totalPages : pageIndex;

                var start = pageSize * pageIndex - pageSize;
                if (start < 0)
                {
                    start = 0;
                }

                jqgrid.total = totalPages;
                jqgrid.page = pageIndex;
                jqgrid.records = count;
                jqgrid.start = start;

                list = logic.GetPaging(sidx, orden, pageIndex, pageSize, where).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return new GenericDouble<JQgrid, T>(jqgrid, list);
        }
        
        protected String GetWhere(string columna, string operacion, string valor)
        {
            var opciones = new Dictionary<string, string>();
            {
                opciones.Add("eq", "=");
                opciones.Add("ne", "<>");
                opciones.Add("lt", "<");
                opciones.Add("le", "<=");
                opciones.Add("gt", ">");
                opciones.Add("ge", ">=");
                opciones.Add("bw", "LIKE");
                opciones.Add("bn", "NOT LIKE");
                opciones.Add("in", "LIKE");
                opciones.Add("ni", "NOT LIKE");
                opciones.Add("ew", "LIKE");
                opciones.Add("en", "NOT LIKE");
                opciones.Add("cn", "LIKE");
                opciones.Add("nc", "NOT LIKE");
            }

            if (string.IsNullOrEmpty(operacion))
            {
                return string.Empty;
            }

            if (operacion.Equals("bw") || operacion.Equals("bn"))
            {
                valor = valor + "%";
            }
            if (operacion.Equals("ew") || operacion.Equals("en"))
            {
                valor = "%" + valor;
            }
            if (operacion.Equals("cn") || operacion.Equals("nc") || operacion.Equals("in") || operacion.Equals("ni"))
            {
                valor = "%" + valor + "%";
            }
            if (opciones.Take(6).ToDictionary(p => p.Key, p => p.Value).ContainsKey(operacion))
            {
                return string.IsNullOrEmpty(valor) ? string.Empty : (columna + " ") + opciones[operacion] + " " + valor;
            }

            return columna + " " + opciones[operacion] + " '" + valor + "'";
        }

        protected GenericDouble<JQgrid, T> ListarSql<T>(IRepository<T> logic, string sidx, string sord, int pageIndex, int pageSize, bool search, string searchField, string searchOper, string searchString, string where) where T : class
        {
            if (string.IsNullOrEmpty(sidx))
            {
                sidx = string.Empty;
            }
            if (string.IsNullOrEmpty(sord))
            {
                sord = "desc";
            }
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 100;
            }

            var jqgrid = new JQgrid();
            IList<T> list;

            try
            {
                int totalPages = 0;

                //if (search)
                //{
                //    @where = @where + GetWhere(searchField, searchOper, searchString);
                //}

                var test = logic.GetPagingBySql(sidx, true, pageIndex, pageSize, where);
                list = test.Lista;

                var count = logic.CountBy();

                if (count > 0 && pageSize > 0)
                {
                    if (count % pageSize > 0)
                    {
                        totalPages = (int)Math.Ceiling((decimal)(count / pageSize));
                        totalPages = (int)(count / pageSize) + 1;
                    }
                    else
                    {
                        totalPages = (int)(count / pageSize);
                    }
                    totalPages = totalPages == 0 ? 1 : totalPages;
                }

                pageIndex = pageIndex > totalPages ? totalPages : pageIndex;

                var start = pageSize * pageIndex - pageSize;
                if (start < 0)
                {
                    start = 0;
                }

                jqgrid.total = totalPages;
                //
                jqgrid.page = pageIndex;
                jqgrid.records = count;
                jqgrid.start = start;

                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return new GenericDouble<JQgrid, T>(jqgrid, list);
        }


        #endregion Paginacion


        #region Control Error

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnException(ExceptionContext filterContext)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            if (filterContext.Exception.GetType() == typeof(TerminoSesionException))
            {
                //redirect la pagina login
            }
            else 
            {
                //error generico
            }

            //logger.Error(string.Format("Controlador:{0}  Action:{1}  Mensaje:{2}", controllerName, actionName, filterContext.Exception.Message));
            
            filterContext.Result = View("Error");
        }

        protected JsonResult MensajeError(string mensaje = "Ocurrio un error al cargar...")
        {
            Response.StatusCode = 404;
            return Json(new JsonResponse { Message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion Control Error

        #region Propiedades

        /// <summary>
        /// usuario actual
        /// </summary>
        //protected Usuario UsuarioActual
        //{
        //    get 
        //    { 
        //        var usuario = (Usuario)System.Web.HttpContext.Current.Session[ConstanteSesion.UsuarioDes]; 
        //        return usuario;
        //    }
        //    set { System.Web.HttpContext.Current.Session.Add(ConstanteSesion.Usuario, value); }
        //}
        protected Usuario UsuarioActual
        {
            get
            {
                //return (Usuario)System.Web.HttpContext.Current.Session[ConstanteSesion.Usuario]; 
                return new Usuario { NombreUsuario = (String)System.Web.HttpContext.Current.Session[ConstanteSesion.UsuarioDes] };
            }
            set { System.Web.HttpContext.Current.Session.Add(ConstanteSesion.Usuario, value); }
        }






        protected T ObtenerSession<T> (string nombre)
        {
            T valor = (T)Session[nombre];
            if (valor == null)
            {
                //Redirect('');
            }
            return valor;
        }
        ///// <summary>
        ///// Logger instance.
        ///// </summary>
        //protected static readonly ILog logger = LogManager.GetLogger(string.Empty);

        public DateTime FechaSistema
        {
            get { return DateTime.Now; }
        }

        public DateTime FechaCreacion
        {
            get { return DateTime.Now; }
        }
        public DateTime HoraInicio
        {
            get { return DateTime.Now; }
        }

        public DateTime FechaModificacion
        {
            get { return DateTime.Now; }
        }

        protected int IdePostulantePreSeleccion
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[ConstanteSesion.IdePostulantePreSele];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.IdePostulantePreSele, value);
            }
        }
        protected int IdeReclutaPersona
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[ConstanteSesion.IdeReclutaPersona];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.IdeReclutaPersona, value);
            }
        }
        protected DatosCargo CargoPerfil
        {
            get 
            {
                return (DatosCargo)System.Web.HttpContext.Current.Session[ConstanteSesion.CargoPerfil];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.CargoPerfil, value);
            }
        }

        protected ListaCriterios ListaCriterioEval 
        {
            get
            {
                return (ListaCriterios)System.Web.HttpContext.Current.Session[ConstanteSesion.HoraInicioEvaluacion];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.HoraInicioEvaluacion, value);
            }
        }

        protected int  PreguntaActual
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[ConstanteSesion.PreguntaActual];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.PreguntaActual, value);
            }
        }

        protected int IdeReclutamientoExamenCategoria
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[ConstanteSesion.IdeReclutamientoExamenCategoria];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.IdeReclutamientoExamenCategoria, value);
            }
        }
        

        protected DateTime HoraInicioEvaluacion
        {
            get
            {
                return (DateTime)System.Web.HttpContext.Current.Session[ConstanteSesion.ListaCriterios];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.ListaCriterios, value);
            }
        }

        protected int TiempoEvaluacion
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[ConstanteSesion.TiempoEvaluacion];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.TiempoEvaluacion, value);
            }
        }

        protected string  ModoAccion
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session[ConstanteSesion.Modo];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.Modo, value);
            }
        }
        protected int IdeGeneral
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[ConstanteSesion.IdeGeneral];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.IdeGeneral, value);
            }
        }

        protected string DetalleValor
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session[ConstanteSesion.DetalleValor];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.DetalleValor, value);
            }
        }

        protected int IdeSolicitudAmpliacion
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[ConstanteSesion.IdeSolicitudAmpliacion];
                //return 4;
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.IdeSolicitudAmpliacion, value);
            }
        }




        protected int IdePostulante
        {
            get
            {
                var idPostulante = System.Web.HttpContext.Current.Session[ConstanteSesion.IdePostulante];
                return (int)(idPostulante==null?0:idPostulante);
                //return 55;
            }
            set { System.Web.HttpContext.Current.Session.Add(ConstanteSesion.IdePostulante, value); }
        }

        #endregion Propiedades


        /// <summary>
        /// obtiene los datos de una lista para mostrar en el jqgrid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logic"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <param name="searchField"></param>
        /// <param name="searchOper"></param>
        /// <param name="searchString"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected GenericDouble<JQgrid, T> GetListar<T>(List<T> logic, string sidx, string sord, int pageIndex, int pageSize, bool search, string searchField, string searchOper, string searchString) where T : class
        {
            if (string.IsNullOrEmpty(sidx))
            {
                sidx = string.Empty;
            }
            if (string.IsNullOrEmpty(sord))
            {
                sord = "desc";
            }
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 100;
            }

            var jqgrid = new JQgrid();
            IList<T> list;

            try
            {
                int totalPages = 0;

              
                var count = logic.Count();

                if (count > 0 && pageSize > 0)
                {
                    if (count % pageSize > 0)
                    {
                        totalPages = (int)Math.Ceiling((decimal)(count / pageSize));
                        totalPages = (int)(count / pageSize) + 1;
                    }
                    else
                    {
                        totalPages = (int)(count / pageSize);
                    }
                    totalPages = totalPages == 0 ? 1 : totalPages;
                }

                pageIndex = pageIndex > totalPages ? totalPages : pageIndex;

                var start = pageSize * pageIndex - pageSize;
                if (start < 0)
                {
                    start = 0;
                }

                jqgrid.total = totalPages;
                jqgrid.page = pageIndex;
                jqgrid.records = count;
                jqgrid.start = start;

               //LIst = logic.GetPaging(sidx, true, pageIndex,pageSize,where).ToList();

               // pageSize * (pageIndex - 1)
                //var lista = new List<Criterio>();
                //var listap = lista.Skip(grid.rows * grid.page).Take(grid.rows);
                //lista.Count();
               // list = logic.Take(pageSize).ToList();
                list = logic.Skip((pageIndex-1) * pageSize).Take(pageSize).ToList();
                //list = logic.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return new GenericDouble<JQgrid, T>(jqgrid, list);
        }

        /// <summary>
        /// Id de solicitud de reemplazo 
        /// </summary>
        protected int IdeSolicitudReemplazo
        {
            get
            {
                return (int)System.Web.HttpContext.Current.Session[ConstanteSesion.IdeSolicitudAmpliacion];
                
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add(ConstanteSesion.IdeSolicitudAmpliacion, value);
            }
        }


        /// <summary>
        /// codifica a base 64
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// decodifica a base 64
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        


    }
}
