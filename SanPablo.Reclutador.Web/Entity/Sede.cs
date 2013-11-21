using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Sede : BaseEntity
    {
        public virtual string CodigoSede { get; set; }
        public virtual string DescripcionSede { get; set; }
    }
}