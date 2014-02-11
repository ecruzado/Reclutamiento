using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class RolOpcion : BaseEntity
    {
        public virtual int IDROLOPCION { get; set; }
        public virtual int IDOPCION { get; set; }
        public virtual int IDROL { get; set; }
        
        public virtual Rol Rol { get; set; }
        public virtual Opcion Opcion { get; set; }
        public virtual string USRCREACION { get; set; }
        public virtual DateTime FECCREACION { get; set; }
        public virtual string USRMODIFICACION { get; set; }
        public virtual DateTime FECMODIFICACION { get; set; }

    }
}