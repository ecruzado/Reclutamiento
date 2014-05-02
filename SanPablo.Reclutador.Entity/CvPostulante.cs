

namespace SanPablo.Reclutador.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web; 
    
    public class CvPostulante : BaseEntity
    {
        /// <summary>
        /// id del postulante
        /// </summary>
        public virtual int IdCvPostulante { get; set; }
        /// <summary>
        /// Apellido paterno del postulante
        /// </summary>
        public virtual string ApePaterno { get; set; }

        /// <summary>
        /// Apellido materno del postulante
        /// </summary>
        public virtual string ApeMaterno { get; set; }

        /// <summary>
        /// Dni del postulante
        /// </summary>
        public virtual string Dni { get; set; }
        /// <summary>
        /// numero de telefono
        /// </summary>
        public virtual string Telefono { get; set; }
        

        /// <summary>
        /// Fecha de cita del postulante
        /// </summary>
        // [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? Fechacita { get; set; }

        /// <summary>
        /// hora de cita
        /// </summary>
        //[DataType(DataType.Time)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public virtual DateTime? HoraCita { get; set; }

        /// <summary>
        /// indicador de citado
        /// </summary>
        public virtual bool IndicadorCitado
        {
            get
            {
                return Citado == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    Citado = Indicador.Si;
                else
                    Citado = Indicador.No;
            }
        }
        /// <summary>
        /// Inidicador de asistencia
        /// </summary>
        public virtual bool IndicadorAsistio
        {
            get
            {
                return Asistio == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    Asistio = Indicador.Si;
                else
                    Asistio = Indicador.No;
            }
        }

      

        /// <summary>
        /// indicador de citado S = SI, N = NO
        /// </summary>
        public virtual string Citado { get; set; }

        /// <summary>
        /// indicador de Asitio S = SI asistio, N = NO asistio
        /// </summary>
        public virtual string Asistio { get; set; }

        /// <summary>
        /// indicador de Asitio S = SI asistio, N = NO asistio
        /// </summary>
        public virtual string Nombre { get; set; }

        /// <summary>
        /// id de solicitud
        /// </summary>
        public virtual int IdSolicitud { get; set; }

        /// <summary>
        /// tipo de solicitud
        /// </summary>
         public virtual string TipSol { get; set; }

         public virtual DateTime FecCreacion { get; set; }
         public virtual string UsrCreacion { get; set; }
         public virtual DateTime FecModificacion { get; set; }
         public virtual string UsrModifcacion { get; set; }
        

    }
}
