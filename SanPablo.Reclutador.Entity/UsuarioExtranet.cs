using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class UsuarioExtranet : BaseEntity
    {
        public virtual int IdUsuario { get; set; }
        public virtual string CodContrasena { get; set; }
        public virtual string CodUsuario { get; set; }
    }
}
