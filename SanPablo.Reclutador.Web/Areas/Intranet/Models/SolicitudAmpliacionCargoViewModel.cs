

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    
    public class SolicitudAmpliacionCargoViewModel
    {
        public SolReqPersonal SolicitudRequerimiento { get; set; }
        public List<DetalleGeneral> Puestos { get; set; }
        public List<Dependencia> Dependencias { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Area> Areas { get; set; }
        public List<Cargo> Cargos { get; set; }
        public List<Rol> Roles { get; set; }

        public Dependencia DependenciaSession { get; set; }
        public Departamento DepartamentoSession { get; set; }
        public Area AreaSession { get; set; }

        public int TotalMaxino { get; set; }
        public string Area { get; set; }
        public string Dependencia { get; set; }
        public string Departamento { get; set; }
        public string EstadoRegistro { get; set; }
        public int IdeSolicitud { get; set; }
        public string Accion { get; set; }
        public List<DetalleGeneral> Sexos { get; set; }
        public List<DetalleGeneral> TiposRequerimientos { get; set; }
        public List<DetalleGeneral> RangoSalariales { get; set; }

        public List<DetalleGeneral> Etapas { get; set; }
        public List<DetalleGeneral> Estados { get; set; }
        public List<DetalleGeneral> TipoPuestos { get; set; }

        public string Pagina { get; set; }

        public string btnVerRanking { get; set; }
        public string btnVerPreSeleccion { get; set; }
        public string btnVerNuevo { get; set; }
        public string btnVerRequerimiento { get; set; }

    }




}