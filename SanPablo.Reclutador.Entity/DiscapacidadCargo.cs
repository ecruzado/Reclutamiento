using SanPablo.Reclutador.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class DiscapacidadCargo
    {
        public virtual int IdeDiscapacidadCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual string TipoDiscapacidad { get; set; }
        public virtual string DescripcionDiscapacidad { get; set; }
        public virtual string PuntajeDiscapacidad { get; set; }
        public virtual string EstadoActivo { get; set; }

    }
}