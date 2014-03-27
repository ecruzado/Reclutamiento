using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SanPablo.Reclutador.Entity
{
   
    public class ReclutamientoPersonaExamen : BaseEntity
    {
        public virtual int IdeReclutamientoPersonaExamen {get; set;}      
        public virtual int IdeReclutamientoPersona {get; set;}      
        public virtual int IdeEvaluacion {get; set;}      
        public virtual string TipoSolicitud {get; set;}      
        public virtual int IdeUsuarioResponsable {get; set;}
        public virtual string UsuarioResponsable { get; set; }
        public virtual string Observacion { get; set; }

        public virtual string IndicadorEntrevistaFinal { get; set; }
        /// <summary>
        /// fecha de programacion de evaluacion
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FechaEvaluacion {get; set;}
        
        /// <summary>
        /// Hora de programacion de evaluacion
        /// </summary>
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")] 
        public virtual DateTime? HoraEvaluacion {get; set;}      

        public virtual int NotaFinal {get; set;}

        public virtual string rutaArchivo { get; set; }
        public virtual string nombreArchivo { get; set; }
        public virtual byte[] Archivo {get; set;}      
        public virtual string ComentarioResultado {get; set;}      
        public virtual string TipoEstadoEvaluacion {get; set;}

        /// <summary>
        /// Datos del reporte
        /// </summary>
        public virtual string DescripcionExamen { get; set; }

        public virtual string TipoExamen { get; set; }

        
        public virtual string EstadoEvaluacion { get; set; }

        public virtual bool EsEntrevistaFinal
        {
            get
            {
                return IndicadorEntrevistaFinal == Indicador.Si? true : false;
            }
            set
            {
                if (value)
                    IndicadorEntrevistaFinal = Indicador.Si;
                else
                    IndicadorEntrevistaFinal = Indicador.No;
            }
        }
    }
}
