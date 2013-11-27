using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Rol : BaseEntity
    {
        public virtual int CodigoRol { get; set; }
        public virtual string NombreRol { get; set; }
        public virtual string EstadoRegistro { get; set; }     	
    }
}