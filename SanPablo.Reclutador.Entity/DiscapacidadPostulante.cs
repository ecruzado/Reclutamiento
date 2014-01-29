using SanPablo.Reclutador.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class DiscapacidadPostulante
    {
        public virtual int IdeDiscapacidadPostulante { get; set; }
        public virtual Postulante Postulante { get; set; }
        //public virtual int IdePostulante { get; set; }
        public virtual string TipoDiscapacidad { get; set; }
        public virtual string DescripcionDiscapacidad { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionTipoDiscapacidad { get; set; }
    }
}