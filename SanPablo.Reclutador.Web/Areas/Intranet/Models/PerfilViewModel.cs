
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class PerfilViewModel
    {
        public Cargo Cargo { get; set; }
        public string Area { get; set; }
        public string Dependencia { get; set; }
        public string Departamento { get; set; }
        public string EstadoRegistro { get; set; }
        public int IdeSolicitud { get; set; }

        public List<DetalleGeneral> Sexos { get; set; }
        public List<DetalleGeneral> TiposRequerimientos { get; set; }
        public List<DetalleGeneral> RangoSalariales { get; set; }
        
    }
}
