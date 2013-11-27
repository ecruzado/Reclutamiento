using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Alternativa : BaseEntity
    {
        public virtual int CodigoAlternativa { get; set; }
        public virtual int CodigoCriterio { get; set; }
        public virtual string NombreAlternativa { get; set; }
        public virtual int Peso { get; set; }
        public virtual string RutaDeImagen { get; set; }
        public virtual string EstadoDeRegistro { get; set; }
    }
}