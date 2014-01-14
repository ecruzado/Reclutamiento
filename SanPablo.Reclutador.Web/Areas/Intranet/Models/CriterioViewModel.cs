using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
     using SanPablo.Reclutador.Entity;
     using System.Collections.Generic;

    public class CriterioViewModel
    {
        public Criterio criterio { get; set; }
        public virtual IList<ItemTabla> tipoCriterio { get; set; }
        public virtual IList<ItemTabla> medicion { get; set; }
        public virtual IList<ItemTabla> estado { get; set; }
                
    }
}