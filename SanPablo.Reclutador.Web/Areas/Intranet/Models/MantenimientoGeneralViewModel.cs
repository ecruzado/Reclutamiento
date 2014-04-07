

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    
    public class MantenimientoGeneralViewModel
    {

        public General TablaGeneral { get; set; }
        public DetalleGeneral TablaDetalleGeneral { get; set; }
        public DetalleGeneral TablaSubDetalle { get; set; }

        public virtual List<General> tipoTablas { get; set; }
        public string Accion {get; set;}
        public string IndSubDetalle { get; set; }
        public string Valor { get; set; }
        public string AccionModel { get; set; }


    }
}