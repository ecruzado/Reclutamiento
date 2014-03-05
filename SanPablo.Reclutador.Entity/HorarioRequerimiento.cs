using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class HorarioRequerimiento :BaseEntity
    {
        public virtual int IdeHorarioRequerimiento { get; set; }
        public virtual SolReqPersonal SolicitudRequerimiento { get; set; }
        public virtual string TipoHorario { get; set; }
        public virtual int PuntajeHorario { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionHorario { get; set; }



    }
}