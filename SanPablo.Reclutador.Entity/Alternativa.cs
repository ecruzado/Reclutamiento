using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Alternativa : BaseEntity
    {
        public virtual int      IdeAlternativa { get; set; }
        public virtual Criterio Criterio { get; set; }
        public virtual string   NombreAlternativa { get; set; }
        public virtual int      Peso { get; set; }
        public virtual string   RutaDeImagen { get; set; }
        public virtual int      NroOrden { get; set; }
        public virtual string   UsrCreacion { get; set; }
        public virtual string   FechaCreacion { get; set; }
        public virtual string   UsrMod { get; set; }
        public virtual string   FechaMod { get; set; }
        public virtual byte[]   Image { get; set; }

    }
}