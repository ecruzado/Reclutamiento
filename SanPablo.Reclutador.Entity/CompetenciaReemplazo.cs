using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class CompetenciaReemplazo : BaseEntity
    {
        public virtual int IdeCompetenciaReemplazo { get; set; }
        public virtual SolReqPersonal SolicitudRequerimiento { get; set; }
        public virtual string TipoCompetencia { get; set; }
        public virtual string EstadoActivo { get; set; }
        public virtual string DescripcionCompetencia { get; set; }

    }
}
