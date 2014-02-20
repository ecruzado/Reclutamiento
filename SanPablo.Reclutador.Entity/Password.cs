using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    public class Password : BaseEntity
    {
        public virtual string PassAnterior { get; set; }
        public virtual string PassNuevo { get; set; }
        public virtual string PassConfirma { get; set; }

    }
}
