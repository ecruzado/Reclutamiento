﻿using System;
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
        /// obtiene la sede, depencia, departamento, area por usuario hace referencia al Objeto SedeNivel
        /// </summary>
        public const string UsuarioSede = "UsuarioSede";
        /// <summary>
        /// obtiene los datos de la tabla usaurio hace referencia al Objeto Usuario
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

        /// <summary>
        /// Codigo de session unico que se genera al intentar crear una solicitud de requerimiento
        /// para asociar los reemplazos a la solicitud una vez se cree la solicitud
        /// </summary>
        public const string codReqSolTemp = "codReqSolTemp";

    }
    public enum TipoDevolucionError 
    {
        Html,
        Json
    }
}