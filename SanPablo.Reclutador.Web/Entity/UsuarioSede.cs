using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class UsuarioSede : BaseEntity
    {
        public virtual int CodigoUsuarioSede { get; set; }
        public virtual Sede Sede { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}