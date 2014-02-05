using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class OfrecemosCargo : BaseEntity
    {
        public virtual int IdeOfrecemosCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual string TipoOfrecimiento { get; set; }
        public virtual string EstadoActivo { get; set; }
        public virtual string DescripcionOfrecimiento { get; set; }
    }
}