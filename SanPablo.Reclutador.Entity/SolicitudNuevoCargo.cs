using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    
    public class SolicitudNuevoCargo : BaseEntity
    {
        public virtual int IdeSolicitudNuevoCargo { get; set; }
        public virtual int IdeSede { get; set; }
        public virtual string CodigoCargo { get; set; }
        public virtual string NombreCargo { get; set; }
        public virtual string DescripcionCargo { get; set; }
        public virtual int NumeroPosiciones { get; set; }
        public virtual string TipoRangoSalarial { get; set; }
        public virtual int IdeDependencia { get; set; }
        public virtual int IdeDepartamento { get; set; }
        public virtual int IdeArea { get; set; }
        public virtual string IndicadorVerSexo { get; set; }
        public virtual string IndicadorVerSalario { get; set; }
        
        public virtual string DescripcionEstudios { get; set; }
        public virtual string DescripcionFunciones { get; set; }
        public virtual string DescripcionCompetencias { get; set; }
        public virtual string DescripcionObservaciones { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaPublicacion { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaExpiracion { get; set; }
        public virtual string  Motivo { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual bool RangoSalarioPublicar
        {
            get
            {
                return IndicadorVerSalario == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    IndicadorVerSalario = Indicador.Si;
                else
                    IndicadorVerSalario = Indicador.No;
            }
        }
    }
}