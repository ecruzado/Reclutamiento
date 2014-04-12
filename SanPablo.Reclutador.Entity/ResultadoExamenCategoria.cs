using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ResultadoExamenCategoria : BaseEntity
    {

        //Datos de la cabezera

        public virtual int IdeReclutamientoExamenCategoria { get; set; }
        public virtual int IdeReclutamientoPersonaExamen { get; set; }
        public virtual int NumeroPreguntas { get; set; }
        public virtual int NotaCategoria { get; set; }
        public virtual string NombreCategoria { get; set; }
        public virtual string DescripcionCategoria { get; set; }
        public virtual int Tiempo { get; set; }

    }
}
