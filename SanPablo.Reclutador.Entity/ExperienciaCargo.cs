using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ExperienciaCargo
    {
        public virtual int IdeExperienciaCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual string TipoExperiencia { get; set; }
        public virtual int CantidadExperiencia { get; set; }
        public virtual int PuntajeExperiencia { get; set; }
        public virtual string EstadoActivo { get; set; }
        
    }
}
