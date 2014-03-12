using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SanPablo.Reclutador.Entity
{
    public class OportunidadLaboral : BaseEntity
    {
        
        public virtual string TipoHorario { get; set; }
        public virtual int IdeCargo { get; set; }
        public virtual int IdeSede { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FecInicial { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FecFinal { get; set; }

        public virtual string SedeDes { get; set; }
        public virtual string CargoDes { get; set; }
        public virtual int NumVacantes { get; set; }
        public virtual string TipoHorarioDes { get; set; }




    }
}
