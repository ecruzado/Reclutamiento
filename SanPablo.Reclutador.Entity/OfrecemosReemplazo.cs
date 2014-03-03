using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class OfrecemosReemplazo : BaseEntity
    {
        public virtual int IdeOfrecemosReemplazo { get; set; }
        public virtual SolReqPersonal SolicitudReemplazo { get; set; }
        public virtual string TipoOfrecimiento { get; set; }
        public virtual string EstadoActivo { get; set; }
        public virtual string DescripcionOfrecimiento { get; set; }
    }
}