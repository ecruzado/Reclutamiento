using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public abstract class BaseEntity
    {
        public virtual DateTime FechaCreacion { get; set; }
        public virtual DateTime FechaModificacion { get; set; }
    }
}