using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Criterio : BaseEntity
    {
        public virtual int CodigoCriterio { get; set; }
        public virtual string NombreCriterio { get; set; }
        public virtual string DescripcionCriterio { get; set; }
        public virtual int Calificacion { get; set;}
        public virtual string TipoMedicion { get; set; }
        public virtual string TipoCriterio { get; set; }
        public virtual int TipoModoRegistro { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}