using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SanPablo.Reclutador.Entity
{
    public class OportunidadLaboral : BaseEntity
    {
        /// <summary>
        /// tipo de horario
        /// </summary>
        public virtual string TipoHorario { get; set; }

        /// <summary>
        /// id del cargo
        /// </summary>
        public virtual int IdeCargo { get; set; }

        /// <summary>
        /// id de la sede
        /// </summary>
        public virtual int IdeSede { get; set; }


        /// <summary>
        /// rango de inicial de la fecha de publicacion
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FecInicial { get; set; }


        /// <summary>
        /// rango final de la fecha de publicacion
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FecFinal { get; set; }

        /// <summary>
        /// decripcion de la sede
        /// </summary>
        public virtual string SedeDes { get; set; }

        /// <summary>
        /// descripcion del cargo
        /// </summary>
        public virtual string CargoDes { get; set; }

        /// <summary>
        /// numero de vacantes
        /// </summary>
        public virtual int NumVacantes { get; set; }

        /// <summary>
        /// descripcion del tipo de horario
        /// </summary>
        public virtual string TipoHorarioDes { get; set; }


        /// <summary>
        /// id del postulante
        /// </summary>
        public virtual int IdPostulante { get; set; }

        /// <summary>
        /// id de la solicitud
        /// </summary>
        public virtual int IdSolocitud { get; set; }

        
        /// <summary>
        /// Tipo de Solicitud
        /// </summary>
        public virtual string TipoSol { get; set; }

        /// <summary>
        /// tipo de puesto
        /// </summary>
        public virtual string TipoPuesto { get; set; }

        /// <summary>
        /// nombre del cargo
        /// </summary>
        public virtual string NombreCargo { get; set; }

        /// <summary>
        /// Fecha de expiracion
        /// </summary>
        public virtual DateTime FechaExpiracion { get; set; }


    }
}
