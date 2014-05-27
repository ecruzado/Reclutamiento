using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    
    public class EmailSol : BaseEntity
    {
        /// <summary>
        /// id de solicitud
        /// </summary>
        public virtual int IdSol  { get; set; }



      
        /// <summary>
        /// id de rol suceso del usuario logueado
        /// </summary>
        public virtual int IdRolSuceso  { get; set; }

        /// <summary>
        /// tipo de solicitud:
        /// N = nuevo,
        /// A  = ampliacion,
        /// R = reemplazo
        /// </summary>
        public virtual string TipSol  { get; set; }

        /// <summary>
        /// Accion del boton
        /// </summary>
        public virtual string AccionBoton  { get; set; }

        /// <summary>
        /// id del rol al que se debe enviar
        /// </summary>
        public virtual string RolSend { get; set; }
        /// <summary>
        /// id de roles a los que se deben copiar
        /// </summary>
        public virtual string RolCopy1 { get; set; }
        public virtual string RolCopy2 { get; set; }
        public virtual string RolCopy3 { get; set; }
        /// <summary>
        /// correo electronico
        /// </summary>
        public virtual string Email { get; set; }


        /// <summary>
        /// id del rol del cual se quiere saber el correo
        /// </summary>
        public virtual int idRol { get; set; }
        /// <summary>
        /// id de la sede que se relaciona al rol
        /// </summary>
        public virtual int idSede { get; set; }
    
    }
}
