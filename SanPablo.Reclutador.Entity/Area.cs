using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Area : BaseEntity
    {
        public virtual int IdeArea { get; set; }
        public virtual Departamento Departamento { get; set; }
        public virtual string NombreArea { get; set; }
        public virtual string EstadoActivo { get; set; }
    }
}
