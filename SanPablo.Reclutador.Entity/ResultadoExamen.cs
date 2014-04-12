using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ResultadoExamen : BaseEntity
    {

        //Datos de la cabezera

        public virtual int IdeREclutamientoPersonaExamen { get; set; }
        public virtual string NombreExamen { get; set; }
        public virtual string DescripcionExamen { get; set; }
        public virtual string TipoExamen { get; set; }
        public virtual int NotaFinal { get; set; }
        public virtual string nombrePostulante { get; set; }

        public virtual List<ResultadoExamenCategoria> Categorias { get; set; }
        public virtual List<ResultadoExamenSubCategoria> SubCategorias { get; set; }
        public virtual List<ResultadoExamenCriterio> Criterios { get; set; }
        public virtual List<ResultadoExamenAlternativa> Alternativas { get; set; }
    }
}
