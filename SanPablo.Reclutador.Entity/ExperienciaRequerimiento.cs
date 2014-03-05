using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ExperienciaRequerimiento : BaseEntity
    {
        public virtual int IdeExperienciaRequerimiento { get; set; }
        public virtual SolReqPersonal SolicitudRequerimiento { get; set; }
        public virtual string TipoExperiencia { get; set; }
        public virtual int CantidadAnhosExperiencia { get; set; }
        public virtual int CantidadMesesExperiencia { get; set; }
        public virtual int PuntajeExperiencia { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionExperiencia { get; set; }
        
    }
}
