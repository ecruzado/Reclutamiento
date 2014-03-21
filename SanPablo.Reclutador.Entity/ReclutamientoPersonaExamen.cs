﻿using System;
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
        public virtual int IdeEvaluacionSolicitudRequerimiento {get; set;}      
        public virtual int IdeEvaluacionCargo {get; set;}      
        public virtual int IdeRolResponsable {get; set;}
        
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
        public virtual string Archivo {get; set;}      
        public virtual string ComentarioResultado {get; set;}      
        public virtual string EstadoEvaluacion {get; set;}      

    }
}
