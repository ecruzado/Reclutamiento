using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class EstudioPostulante
    {
        public virtual int IdeEstudiosPostulante { get; set; }
        public virtual Postulante Postulante { get; set; }
        public virtual string TipTipoInstitucion { get; set; }
        public virtual string TipoNombreInstitucion { get; set; }
        public virtual string NombreInstitucion { get; set; }
        public virtual string TipoArea { get; set; }
        public virtual string TipoEducacion { get; set; }
        public virtual string TipoNivelAlcanzado { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaEstudioInicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaEstudioFin { get; set; }
        public virtual string IndicadorActualmenteEstudiando { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionTipoInstitucion { get; set; }
        public virtual string DescripcionNombreInstitucion { get; set; }
        public virtual string DescripcionArea { get; set; }
        public virtual string DescripcionEducacion { get; set; }
        public virtual string DescripcionNivelAlcanzado { get; set; }


        public virtual bool ActualmenteEstudiando
        {
            get
            {
                return IndicadorActualmenteEstudiando == Indicador.Si? true : false;
            }
            set
            {
                if (value)
                    IndicadorActualmenteEstudiando = Indicador.Si;
                else
                    IndicadorActualmenteEstudiando = Indicador.No;
            }
        }

    }
}
