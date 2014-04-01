using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SanPablo.Reclutador.Entity
{
   
    public class ReclutamientoPersonaAlternativa : BaseEntity
    {

        public virtual int IdeReclutamientoPersonaAlternativa { get; set; }
        public virtual int IdeReclutaPersonaCriterio { get; set; }
        public virtual int IdeAlternativa { get; set; }

        public virtual bool IndAlternativa { get; set; }

    }
}
