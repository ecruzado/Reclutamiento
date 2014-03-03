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
        /// <summary>
        /// obtiene la sede, depencia, departamento, area por usuario
        /// </summary>
        public const string UsuarioSede = "UsuarioSede";
        /// <summary>
        /// obtiene los datos de la tabla usaurio
        /// </summary>
        public const string ObjUsuario = "ObjUsuario";

        /// <summary>
        /// obtiene los datos de un usuario de extranet
        /// </summary>
        public const string ObjUsuarioExtranet = "ObjUsuarioExtranet";

        /// <summary>
        /// obtiene la lista de personal de reemplazo
        /// </summary>
        public const string ListaReemplazo = "ListaReemplazo";
        public const string cantRegListaReemplazo = "cantRegListaReemplazo";

        public const string IdePostulante = "IdePostulante";
        public const string CargoPerfil = "CargoPerfil";
    }
    public enum TipoDevolucionError 
    {
        Html,
        Json
    }
}