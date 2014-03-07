
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class LogSolicitudAmpliacionCargoViewModel
    {
        public SolReqPersonal SolicitudRequerimiento { get; set; }
        public LogSolReqPersonal LogSolicitudAmpliacion { get; set; }
        public Boolean Aprobado { get; set; }
        
        
       
        
    }
}
