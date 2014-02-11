using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Opcion : BaseEntity
    {
        public virtual int IDITEM { get; set; }
        public virtual int IDOPCION { get; set; }
        public virtual int IDOPCIONPADRE { get; set; }
        public virtual string DSCOPCION { get; set; }
        public virtual string FLGHABILITADO { get; set; }
        public virtual string DSCICONO { get; set; }
        public virtual string DSCURL { get; set; }
        public virtual string DESCRIPCION { get; set; }
        public virtual string USRCREACION { get; set; }
        public virtual DateTime FECCREACION { get; set; }
        public virtual string USRMODIFICACION { get; set; }
        public virtual DateTime FECMODIFICACION { get; set; }


        
        
        //public Opcion()
        //{
        //    Roles = new List<Rol>();
        //}
        //public virtual void AgregarRoles (Rol rol)
        //{
        //    rol.Opciones.Add(this);
        //    Roles.Add(rol);
        //}
    }
}