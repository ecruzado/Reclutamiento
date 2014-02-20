
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class SolicitudNuevoCargoViewModel
    {
        public SolicitudNuevoCargo SolicitudNuevoCargo { get; set; }
        
        public List<Dependencia> Dependencias { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Area> Areas { get; set; }
        public List<DetalleGeneral> RangosSalariales { get; set; }
        public List<SolicitudNuevoCargo> Cargos { get; set; }
        public List<DetalleGeneral> Estados { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
    }
}
