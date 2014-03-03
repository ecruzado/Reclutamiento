

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    
    public class SolicitudAmpliacionCargoViewModel
    {
        public SolReqPersonal SolReqPersonal { get; set; }
        public List<DetalleGeneral> Puestos { get; set; }
        public List<Dependencia> Dependencias { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Area> Areas { get; set; }
        public List<Cargo> Cargos { get; set; }

        public Dependencia DependenciaSession { get; set; }
        public Departamento DepartamentoSession { get; set; }
        public Area AreaSession { get; set; }

    }




}