using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class ExamenPorCategoria : BaseEntity
    {
        public virtual int CodigoExamenPorCAtegoria { get; set; }
        public virtual int CodigoExamen { get; set; }
        public virtual int CodigoCategoria { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}