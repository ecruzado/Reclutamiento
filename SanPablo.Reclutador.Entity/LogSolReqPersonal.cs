

namespace SanPablo.Reclutador.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    
    public class LogSolReqPersonal : BaseEntity
    {
      
        /// <summary>
        /// id del log
        /// </summary>
        public virtual int IdeLogSolReqPersonal { get; set; }
        /// <summary>
        /// id de la solicitud
        /// </summary>
        public virtual int IdeSolReqPersonal { get; set; }
        /// <summary>
        /// tipo de etapa
        /// </summary>
        public virtual string TipEtapa { get; set; }
        /// <summary>
        /// rol responsable
        /// </summary>
        public virtual int RolResponsable { get; set; }
        /// <summary>
        /// usuario responsable
        /// </summary>
        public virtual int UsResponsable { get; set; }

        /// <summary>
        /// observacion de rechazo
        /// </summary>
        public virtual string Observacion { get; set; }
        /// <summary>
        /// fecha de suceso
        /// </summary>
        public virtual DateTime? FecSuceso { get; set; }
        /// <summary>
        /// usuario de suceso
        /// </summary>
        public virtual int UsrSuceso { get; set; }

        /// <summary>
        /// rol de usuario de suceso
        /// </summary>
        public virtual int RolSuceso { get; set; }

        /// <summary>
        /// indicador de aprobado
        /// </summary>
        public virtual Boolean Aprobado { get; set; }

        

    
    }

}
