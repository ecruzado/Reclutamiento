using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    public class UsuarioRolSede : BaseEntity
    {
       
        public virtual int IdUsuarolSede { get; set; }
        public virtual int IdSede { get; set; }
        public virtual int IdUsuario { get; set; }
        public virtual int IdRol { get; set; }
        public virtual string RolDes { get; set; }
        public virtual string SedeDes { get; set; }
    
    }
}
