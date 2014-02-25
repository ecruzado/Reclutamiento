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
        public const string UsuarioSede = "UsuarioSede";
        public const string ObjUsuario = "ObjUsuario";
        public const string ObjUsuarioExtranet = "ObjUsuarioExtranet";


        public const string IdePostulante = "IdePostulante";
        public const string CargoPerfil = "CargoPerfil";
    }
    public enum TipoDevolucionError 
    {
        Html,
        Json
    }
}