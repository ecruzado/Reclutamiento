using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ExperienciaPostulante
    {
        public virtual int IdeExperienciaPostulante { get; set; }
        public virtual Postulante Postulante { get; set; }
        public virtual int IdePostulante { get; set; }
        public virtual string NombreEmpresa { get; set; }
        public virtual string TipoCargoTrabajo { get; set; }
        public virtual string NombreCargoTrabajo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FechaTrabajoInicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FechaTrabajoFin { get; set; }
        public virtual string IndicadorActualmenteTrabajo { get; set; }
        public virtual string TiempoDeServicio { get; set; }
        public virtual string TipoMotivoCese { get; set; }
        public virtual string NombreReferente { get; set; }
        public virtual string CorreoReferente { get; set; }
        [DataType(DataType.PhoneNumber)]
        public virtual int? NumeroMovilReferencia { get; set; }
        public virtual string TipoCargoTrabajoReferente { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2,3})\)?([-]{1})([0-9]{3})([-]{1})([0-9]{3,4})$", ErrorMessage = "Formato de telefono no válido : (054)-124-1245")]
        public virtual string NumeroFijoInstitucionReferente { get; set; }

        public virtual int? NumeroAnexoInstitucionReferente { get; set; }
        
        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionCargoTrabajo { get; set; }
        public virtual string DescripcionMotivoCese { get; set; } 
        public virtual string DescripcionCargoReferente { get; set; }

        public virtual string FuncionesDesempenadas { get; set; }

        public virtual bool ActualmenteTrabajando
        {
            get
            {
                return IndicadorActualmenteTrabajo == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    IndicadorActualmenteTrabajo = Indicador.Si;
                else
                    IndicadorActualmenteTrabajo = Indicador.No;
            }
        }

        [DataType(DataType.Date)]
        [RegularExpression(@"^\d{2}\/\d{4}$", ErrorMessage = "Ingresar en el formato 'mm/aaaa'")]
        public virtual string FechaInicio
        {
            get
            {
                return FechaTrabajoInicio == null ? "" : String.Format("{0:dd/MM/yyyy}", FechaTrabajoInicio).Substring(3, 7);
            }
            set
            {
                if (value == null)
                    FechaTrabajoInicio = null;
                else
                    FechaTrabajoInicio = Convert.ToDateTime(value.Insert(0, "01/"));
            }
        }

        [DataType(DataType.Date)]
        [RegularExpression(@"^\d{2}\/\d{4}$", ErrorMessage = "Ingresar en el formato 'mm/aaaa'")]
        public virtual string FechaFin
        {
            get
            {
                return FechaTrabajoFin == null ? "" : String.Format("{0:dd/MM/yyyy}", FechaTrabajoFin).Substring(3, 7);
            }
            set
            {
                if (value == null)
                    FechaTrabajoFin = null;
                else
                    FechaTrabajoFin = Convert.ToDateTime(value.Insert(0, "01/"));
            }
        }

        
    }
}
