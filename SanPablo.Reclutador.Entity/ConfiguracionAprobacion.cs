using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ConfiguracionAprobacion :BaseEntity 
    {
        public virtual int CodigoConfiguracionAprobacion { get; set; }        	
        public virtual string CodigoProceso { get; set;}
        public virtual int NumeroSuceso { get; set; }
        public virtual int NumeroSucesoSiguiente { get; set; }
        public virtual int CodigoCargo { get; set; } 
    }
}