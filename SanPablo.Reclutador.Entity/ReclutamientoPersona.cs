using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SanPablo.Reclutador.Entity
{
   
    public class ReclutamientoPersona : BaseEntity
    {
        /// <summary>
        /// Id del postulante
        /// </summary>
        public virtual int IdePostulante { get; set; }
        /// <summary>
        /// id de reclutamiento persona
        /// </summary>
        public virtual int IdeReclutaPersona { get; set; }
        /// <summary>
        /// id de solicitud, es cualquier solictud se diferencia por el tipo de solicitud(tipSol)
        /// </summary>
        public virtual int IdeSol { get; set; }

        /// <summary>
        /// tipo de solicitud 01 nuevo, 02 ampliacion , 03 reemplazo
        /// </summary>
        public virtual string TipSol { get; set; }
        /// <summary>
        /// id de cargo asociado a la solicitud
        /// </summary>
        public virtual int IdeCargo { get; set; }

        /// <summary>
        /// id del cv
        /// </summary>
        public virtual int IdeCv { get; set; }

        /// <summary>
        /// estado del postulante relacionado a un reclutamiento 
        /// </summary>
        public virtual string EstActivo { get; set; }
        /// <summary>
        /// estados del postulante
        /// </summary>
        public virtual string EstPostulante { get; set; }

        /// <summary>
        /// indicador de contactado
        /// </summary>
        public virtual string IndContactado { get; set; }

        /// <summary>
        /// referencia a indicador de contactado
        /// </summary>
        public virtual bool IndicadorContactado
        {
            get
            {
                return IndContactado == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    IndContactado = Indicador.Si;
                else
                    IndContactado = Indicador.No;
            }
        }
        


        /// <summary>
        /// numero de evaluaciones por la que paso el postulante
        /// </summary>
        public virtual int Evaluacion { get; set; }

        /// <summary>
        /// puntaje total obtenido del proceso de preseleccion automatica
        /// </summary>
        public virtual int PtoTotal { get; set; }

        /// <summary>
        /// comentario de la preseleccion
        /// </summary>
        public virtual string Comentario { get; set; }

        /// <summary>
        /// tipo de puesto es el turno en que va a trabajar el postulante
        /// </summary>
        public virtual string TipPuesto { get; set; }
        
        /// <summary>
        /// sede
        /// </summary>
        public virtual int IdSede { get; set; }

        /// <summary>
        /// apellido paterno
        /// </summary>
        public virtual string ApePaterno { get; set; }

        /// <summary>
        /// apellido matenro
        /// </summary>
        public virtual string ApeMaterno { get; set; }

        /// <summary>
        /// nombre
        /// </summary>
        public virtual string Nombre { get; set; }


        /// <summary>
        /// Apellidos concatenacion de apellido paterno + materno
        /// </summary>
        public virtual string Apellidos { get; set; }

        /// <summary>
        /// concatenacion de nombres
        /// </summary>
        public virtual string Nombres { get; set; }

        /// <summary>
        /// descripcion de estados del postulante
        /// </summary>
        public virtual string DesEstadoPostulante { get; set; }

        /// <summary>
        /// telefono fijo
        /// </summary>
        public virtual string FonoFijo { get; set; }

        /// <summary>
        /// telefono movil
        /// </summary>
        public virtual string FonoMovil { get; set; }

        /// <summary>
        /// numero de evaluacion que paso el postulante
        /// </summary>
        public virtual string EvalPostulante { get; set; }

        /// <summary>
        /// fecha de creacion
        /// </summary>
        public virtual DateTime FecCreacion { get; set; }

        /// <summary>
        /// usuario de modificacion
        /// </summary>
        public virtual string UsrModifica { get; set; }

        /// <summary>
        /// fecha de modificacion
        /// </summary>
        public virtual DateTime FecModifica { get; set; }

        /// <summary>
        /// indicador de aprobacion si es S color  por defecto si es N color rojo
        /// </summary>
        public virtual string IndAprobacion { get; set; }

        /// <summary>
        /// Numerod de vacantes de las solicitud
        /// </summary>
        public virtual int NumVacantes { get; set; }
        
        

    }
}
