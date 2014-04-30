
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class LogSolicitudNuevoCargoViewModel
    {
        public SolicitudNuevoCargo SolicitudNuevoCargo { get; set; }
        public LogSolicitudNuevoCargo LogSolicitudNuevoCargo { get; set; }
        public Boolean Aprobado { get; set; }
        public string rechazadoObservado { get; set; }
        
        
       
        
    }
}
