using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class EstudioPostulante
    {
        public virtual int IdeEstudiosPostulante { get; set; }
        public virtual Persona Postulante { get; set; }
        //public virtual int IdePostulante { get; set; }
        public virtual string TipTipoInstitucion { get; set; }
        public virtual string TipoNombreInstitucion { get; set; }
        public virtual string NombreInstitucion { get; set; }
        public virtual string TipoArea { get; set; }
        public virtual string TipoEducacion { get; set; }
        public virtual string TipoNivelAlcanzado { get; set; }
        public virtual DateTime FechaEstudioInicio { get; set; }
        public virtual DateTime FechaEstudioFin { get; set; }
        public virtual string IndicadorActualmenteEstudiando { get; set; }
    }
}
