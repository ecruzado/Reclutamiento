using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SanPablo.Reclutador.Entity
{
   
    public class ReclutamientoPersonaExamenCategoria : BaseEntity
    {

        public virtual int IdeReclutamientoPersonaExamenCategoria   { get; set; }
        public virtual int IdeReclutaPersona  { get; set; }
        public virtual int IdeReclutaPersonaExamen  { get; set; }
        public virtual int IdeExamenPorCategoria  { get; set; }
        public virtual string Estado  { get; set; }
        public virtual int Nota { get; set; }
        public virtual string EstadoActivo { get; set; }

    }
}
