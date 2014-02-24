

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    

    public class UsuarioRolSedeViewModel
    {
        public UsuarioRolSede UsuarioRolSede { get; set; }
        public Usuario Usuario { get; set; }
        public Password Password { get; set; }
        public TipoRequerimiento TipoReq { get; set; }
        public SedeNivel SedeNivel { get; set; }
        public List<Rol> TipRol { get; set; }
        public List<Sede> TipSede { get; set; }
        public List<Dependencia> Dependencias { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Area> Areas { get; set; }
      
        public int IdUsuario { get; set; }
        public int IdRolUsuario { get; set; }
        public string IndSede { get; set; }

    }
}