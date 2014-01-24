using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class CriterioPorSubcategoria : BaseEntity
    {
        public virtual int IDECRITERIOXSUBCATEGORIA { get; set; }
        public virtual Criterio Criterio { get; set; }
        public virtual SubCategoria SubCategoria { get; set; }
        public virtual int IDESUBCATEGORIA { get; set; }
        public virtual int PUNTAMAXIMO { get; set; }
        public virtual int PRIORIDAD { get; set; }
        public virtual string ESTREGISTRO { get; set;}
        public virtual string USRCREACION { get; set;}
        public virtual DateTime FECCREACION { get; set;}
        public virtual string USRMODIFICA { get; set;}
        public virtual DateTime FECMODIFICA { get; set; }

    }
}