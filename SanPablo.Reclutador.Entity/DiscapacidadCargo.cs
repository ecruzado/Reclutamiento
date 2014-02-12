using SanPablo.Reclutador.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class DiscapacidadCargo : BaseEntity
    {
        public virtual int IdeDiscapacidadCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual string TipoDiscapacidad { get; set; }
        public virtual string DescripcionDiscapacidad { get; set; }
        public virtual int PuntajeDiscapacidad { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionTipoDiscapacidad { get; set; }

    }
}