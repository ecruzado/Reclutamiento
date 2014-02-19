using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class MenuItem : BaseEntity
    {
        public virtual int IDOPCIONPADRE { get; set; }
        public virtual int IDOPCION { get; set; }
        public virtual string DESCRIPCION { get; set; }
        public virtual string DSCURL { get; set; }
        public virtual int IDROL { get; set; }

    }
}
