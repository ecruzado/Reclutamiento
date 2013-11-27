using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Config_Aprobacion :BaseEntity 
    {
        public virtual int CodigoConfiguracionAprobacion { get; set; }        	
        public virtual string CodigoDeProceso { get; set;}
        public virtual int NumeroDeSuceso { get; set; }
        public virtual int NumeroDeSucesosIG { get; set; }
        public virtual int IdeCargo { get; set; } 
    }
}