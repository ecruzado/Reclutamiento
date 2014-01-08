using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class EstudioPostulante
    {
        public virtual int ideEstudiosPostulante { get; set; }
        public virtual Persona postulante { get; set; }
        public virtual string tipTipoInstitucion { get; set; }
        public virtual string tipNombreInstitucion { get; set; }
        public virtual string nombreInstitucion { get; set; }
        public virtual string tipoArea { get; set; }
        public virtual string tipoEducacion { get; set; }
        public virtual string tipoNivelAlcanzado { get; set; }
        public virtual DateTime fechaEstudioInicio { get; set; }
        public virtual DateTime fechaEstudioFin { get; set; }
        public virtual string indicadorActualmenteEstudiando { get; set; }
    }
}
