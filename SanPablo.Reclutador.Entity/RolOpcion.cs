using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class RolOpcion : BaseEntity
    {
        public virtual int CodigoRolOpcion { get; set; }
        public virtual Rol Rol { get; set; }
        public virtual Opcion Opcion { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}