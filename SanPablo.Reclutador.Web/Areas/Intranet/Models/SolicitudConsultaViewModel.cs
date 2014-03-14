

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    
    public class SolicitudConsultaViewModel
    {
        public SolReqPersonal SolicitudRequerimiento { get; set; }
        public List<DetalleGeneral> Puestos { get; set; }
        public List<Dependencia> Dependencias { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Area> Areas { get; set; }
        public List<Cargo> Cargos { get; set; }
        public List<Rol> Roles { get; set; }

        public List<DetalleGeneral> Estados { get; set; }
        public List<DetalleGeneral> Etapas { get; set; }
        public List<DetalleGeneral> TiposSolicitudes { get; set; }

    }

}