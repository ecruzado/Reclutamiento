using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class OfrecemosRequerimiento : BaseEntity
    {
        public virtual int IdeOfrecemosRequerimiento { get; set; }
        public virtual SolReqPersonal SolicitudRequerimiento { get; set; }
        public virtual string TipoOfrecimiento { get; set; }
        public virtual string EstadoActivo { get; set; }
        public virtual string DescripcionOfrecimiento { get; set; }
    }
}