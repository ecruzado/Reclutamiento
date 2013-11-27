using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Usuario : BaseEntity
    {
        public virtual int CodigoUsuario { get; set; }
        public virtual int CodigoRol { get; set; }
        public virtual int CodigoEmpleado { get; set; }
        public virtual string NombreUsuario {  get; set; }
        public virtual string ClaveUsuario { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}