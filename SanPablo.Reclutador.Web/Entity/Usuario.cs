using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Usuario : BaseEntity
    {
        public virtual int CodigoUsuario { get; set; }
        public virtual Rol Rol { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual string NombreUsuario {  get; set; }
        public virtual string ClaveUsuario { get; set; }
        public virtual string EstadoRegistro { get; set; }
        public virtual IList<Sede> UsuarioDeLasSedes { get; protected set; }

        public Usuario()
        {
            UsuarioDeLasSedes = new List<Sede>();
        }
    }
}