using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class SubCategoria : BaseEntity
    {
        public virtual int CodigoSubCategoria { get; set; }
        public virtual int CodigoCategoria { get; set; }
        public virtual string NombreSubCategoria { get; set; }
        public virtual string DescripcionSubCategoria { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}