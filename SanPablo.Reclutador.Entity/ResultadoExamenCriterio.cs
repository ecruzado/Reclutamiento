using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ResultadoExamenCriterio : BaseEntity
    {

        //Datos de la cabezera
        public virtual int IdeReclutaPersonaCriterio { get; set; }
        public virtual int IdeReclutamientoExamenCategoria { get; set; }
        public virtual int IdeCriterioSubCategoria { get; set; }
        public virtual int IdeSubCategoria { get; set; }
        public virtual string Pregunta { get; set; }
        public virtual string TipoModo { get; set; }
        public virtual byte[] ImagenCriterio { get; set; }
        public virtual string IndicadorRespuesta { get; set; }
        public virtual int PuntajeTotal { get; set; }
    }
}
