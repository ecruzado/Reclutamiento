

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    

    public class CriterioViewModel
    {
        public Criterio Criterio { get; set; }
        public virtual string IndVisual { get; set; }
        public Alternativa Alternativa { get; set; }
        public CriterioPorSubcategoria CriterioPorSubcategoria { get; set; }
        public virtual List<DetalleGeneral> TipoCriterio { get; set; }
        public virtual List<DetalleGeneral> Medicion { get; set; }
        public virtual List<DetalleGeneral> Estado { get; set; }
        public virtual List<DetalleGeneral> TipoModo { get; set; }
        public virtual List<DetalleGeneral> TipoCalificacion { get; set; }
        public HttpPostedFileBase image { get; set; }
        public HttpPostedFileBase ImagenAlternativa { get; set; }
                
    }
}