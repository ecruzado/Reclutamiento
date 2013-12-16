using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ExamenPorCategoria : BaseEntity
    {
        public virtual int CodigoExamenPorCAtegoria { get; set; }
        public virtual Examen Examen { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}