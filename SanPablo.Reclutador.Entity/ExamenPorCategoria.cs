using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ExamenPorCategoria : BaseEntity
    {
        public virtual int IdeExamenxCategoria { get; set; }
        public virtual string EstActivo { get; set; }
        public virtual string UsrCreacion { get; set; }
        public virtual DateTime FecCreacion { get; set; }
        public virtual string UsrModifica { get; set; }
        public virtual DateTime FecModifica { get; set; }
        public virtual int IdeSede { get; set; }
        public virtual Examen Examen { get; set; }
        public virtual Categoria Categoria { get; set; }

    }
}