using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class CriterioPorSubcategoria : BaseEntity
    {
        public virtual int CodigoCriterioPorSubcategoria { get; set; }
        public virtual Criterio Criterio { get; set; }
        public virtual SubCategoria SubCategoria { get; set; }
        public virtual int PuntajeMaximo { get; set; }
        public virtual int Prioridad { get; set; }
        public virtual string EstadoRegistro { get; set;}
    }
}