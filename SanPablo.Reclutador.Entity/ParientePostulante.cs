using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ParientePostulante: BaseEntity
    {
        public virtual int IdeParientePostulante { get; set; }
        public virtual Postulante Postulante { get; set; }
        public virtual int IdePostulante { get; set; }
        public virtual string ApellidoPaterno { get; set ; }
        public virtual string ApellidoMaterno { get; set; }
        public virtual string Nombres { get; set; }
        public virtual string TipoDeVinculo { get; set; }
       
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FechaNacimiento { get; set; }

        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionVinculo { get; set; }

    }
}