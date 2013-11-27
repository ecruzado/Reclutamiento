using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class RolOpcion : BaseEntity
    {
        public virtual int CodigoRolOpcion { get; set; }
        public virtual int CodigoRol { get; set; }
        public virtual int CodigoOpcion { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}