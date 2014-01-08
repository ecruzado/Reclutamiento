using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Ubigeo : BaseEntity
    {
        public virtual int IdeUbigeo { get; set; }
        public virtual Ubigeo UbigeoPadre { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Codigo { get; set; }
    }
}