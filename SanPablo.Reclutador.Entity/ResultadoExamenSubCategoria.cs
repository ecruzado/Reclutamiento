using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ResultadoExamenSubCategoria : BaseEntity
    {

        //Datos de la cabezera
        public virtual int IdeSubCategoria { get; set; }
        public virtual int IdeReclutamientoExamenCategoria { get; set; }
        public virtual int IdeReclutamientoPersonaExamen { get; set; }
        public virtual string NombreSubCategoria { get; set; }
        public virtual string DescripcionSubCategoria { get; set; }
        public virtual int OrdenImpresion { get; set; }

    }
}
