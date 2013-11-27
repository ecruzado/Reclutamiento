using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class CriterioPorSubcategoria : BaseEntity
    {
        public virtual int CodigoCriterioPorSubcategoria { get; set; }
        public virtual int CodigoCriterio { get; set; }
        public virtual int CodigoSubCategoria { get; set; }
        public virtual int PuntajeMaximo { get; set; }
        public virtual int Prioridad { get; set; }
        public virtual string EstadoRegistro { get; set;}
    }
}