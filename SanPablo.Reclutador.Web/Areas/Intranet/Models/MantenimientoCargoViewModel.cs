
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class MantenimientoCargoViewModel
    {
        public Cargo Cargo { get; set; }
        public string Area { get; set; }
        public string Dependencia { get; set; }
        public string Departamento { get; set; }
        public string EstadoRegistro { get; set; }
        public int IdeSolicitud { get; set; }

        public int TotalMaximo { get; set; }
        public int TotalMinimo { get; set; }

        public List<DetalleGeneral> Sexos { get; set; }
        public List<DetalleGeneral> TiposRequerimientos { get; set; }
        public List<DetalleGeneral> RangoSalariales { get; set; }
        public List<DetalleGeneral> Estados { get; set; }

        public List<Dependencia> Dependencias { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Area> Areas { get; set; }

        public List<Cargo> Cargos { get; set; }
        public string Accion { get; set; }

        public string btnVerConsultar { get; set; }
        public string btnVerEditar { get; set; }
        public string btnVerActivarDesc { get; set; }


        public List<Edad> ListaEdad { get; set; }

        public string indVisibilidad { get; set; }

        
    }
}
