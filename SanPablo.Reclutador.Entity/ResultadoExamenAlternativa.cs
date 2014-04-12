using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ResultadoExamenAlternativa : BaseEntity
    {

        //Datos de la cabezera
        public virtual int IdeReclutaPersonaCriterio { get; set; }
        public virtual int IdeCriterio { get; set; }
        public virtual int IdeCriterioSubCategoria { get; set; }
        public virtual int IdeAlternativa { get; set; }
        public virtual string Alternativa { get; set; }
        public virtual byte[] ImagenAlternativa { get; set; }
        public virtual int Peso { get; set; }
        public virtual string Respuesta { get; set; }
    }
}
