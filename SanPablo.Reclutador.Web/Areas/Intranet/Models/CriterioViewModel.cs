

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class CriterioViewModel
    {
        public Criterio Criterio { get; set; }
        public virtual List<DetalleGeneral> TipoCriterio { get; set; }
        public virtual List<DetalleGeneral> Medicion { get; set; }
        public virtual List<DetalleGeneral> Estado { get; set; }
                
    }
}