

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    
    public class SolicitudRempCargoViewModel
    {
        public SolReqPersonal SolReqPersonal { get; set; }
        public Reemplazo Reemplazo { get; set; }
        public LogSolReqPersonal LogSolReqPersonal { get; set; }

        public List<DetalleGeneral> listaTipPuesto { get; set; }
        public List<DetalleGeneral> listaTipVacante { get; set; }
        public List<Dependencia> Dependencias { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Area> Areas { get; set; }
        public List<Cargo> listaTipCargo { get; set; }

        public List<Dependencia> listaDependencia { get; set; }
        public List<Departamento> listaDepartamento { get; set; }
        public List<Area> listaArea { get; set; }
        public List<Rol> listaRol { get; set; }
        public List<DetalleGeneral> listaEtapas { get; set; }
        public List<DetalleGeneral> listaEstados { get; set; }
        
      
        public string Accion { get; set; }
        public string AccionPopup { get; set; }

        public string Visualiza { get; set; }

    }




}