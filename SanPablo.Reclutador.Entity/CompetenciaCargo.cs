using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class CompetenciaCargo : BaseEntity
    {
        public virtual int IdeCompetenciaCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual string TipoCompetencia { get; set; }
        public virtual string EstadoActivo { get; set; }
        public virtual string DescripcionCompetencia { get; set; }

    }
}
