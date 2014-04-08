using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SanPablo.Reclutador.Entity
{
   
    public class ReclutamientoPersonaCriterio : BaseEntity
    {

        public virtual int IdeReclutamientoPersonaCriterio { get; set; }
        public virtual int IdeReclutaPersona { get; set; }
        public virtual int IdeCriterioXSubcategoria { get; set; }
        public virtual int IdeReclutamientoExamenCategoria {get;set;}
        public virtual string IndicadorRespuesta { get; set; }
        public virtual int PuntajeTotal  { get; set; }
        

    }
}
