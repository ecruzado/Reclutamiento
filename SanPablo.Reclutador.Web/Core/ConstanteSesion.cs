using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Core
{
    public sealed class ConstanteSesion
    {
        public const string Usuario = "Usuario";
        public const string Rol = "Rol";
        public const string Sede = "Sede";
        public const string SedeDes = "SedeDes";
        public const string RolDes = "RolDes";
        public const string UsuarioDes = "UsuarioDes";
        public const string Modo = "Modo";
        public const string IdePostulantePreSele = "IdePostPreSel";
        public const string IdeReclutaPersona = "IdeReclutaPersona";
        public const string ListaCriterios = "ListaCriterios";

        public const string HoraInicioEvaluacion = "HoraInicioEvaluacion";
        public const string TiempoEvaluacion = "TiempoEvaluacion";
        public const string IdeReclutamientoExamenCategoria = "IdeReclutamientoExamenCategoria";
        public const string PreguntaActual = "PreguntaActual";

        /// <summary>
        /// Guarda el identificador de la tabla general
        /// </summary>
        public const string IdeGeneral = "IdeGeneral";

        /// <summary>
        /// Guarda el campo valor del detalle en session
        /// </summary>
        public const string DetalleValor = "DetalleValor";

        /// <summary>
        /// obtiene la sede, depencia, departamento, area por usuario hace referencia al Objeto SedeNivel
        /// </summary>
        
        public const string UsuarioSede = "UsuarioSede";
        /// <summary>
        /// obtiene los datos de la tabla usaurio hace referencia al Objeto Usuario
        /// </summary>
       
        public const string ObjUsuario = "ObjUsuario";

        /// <summary>
        /// obtiene los datos de un usuario de extranet hace referencia al objeto Usuario
        /// </summary>
        public const string ObjUsuarioExtranet = "ObjUsuarioExtranet";

        /// <summary>
        /// obtiene la lista de personal de reemplazo
        /// </summary>
        public const string ListaReemplazo = "ListaReemplazo";
        public const string cantRegListaReemplazo = "cantRegListaReemplazo";

        public const string IdePostulante = "IdePostulante";
        public const string CargoPerfil = "CargoPerfil";

        public const string IdeSolicitudAmpliacion = "ideSolicitudAmp";
        /// <summary>
        /// Codigo de session unico que se genera al intentar crear una solicitud de requerimiento
        /// para asociar los reemplazos a la solicitud una vez se cree la solicitud
        /// </summary>
        public const string codReqSolTemp = "codReqSolTemp";

        /// <summary>
        /// id de la solicitud de reemplazo
        /// </summary>
        public const string IdeSolicitudReemplazo = "IdeSolicitudReemplazo";

        /// <summary>
        /// obtiene los filtros de un reporte de seleccion, clase REPORTE
        /// </summary>
        public const string ReporteSeleccion = "ReporteSeleccion";

    }
    public enum TipoDevolucionError 
    {
        Html,
        Json
    }
}