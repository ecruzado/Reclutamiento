using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Rol : BaseEntity
    {
        public virtual string CodRol { get; set; }
        public virtual string DscRol { get; set; }
        public virtual string FlgSede { get; set; }
        public virtual string FlgEstado { get; set; }
        public virtual int IdRol { get; set; }

        //public virtual IList<Opcion> Opciones { get; set; }
       
        //public Rol()
        //{
        //    Opciones = new List<Opcion>();
        //}
        //public virtual void AgregarOpcion(Opcion opcion)
        //{
        //    opcion.Roles.Add(this);
        //    Opciones.Add(opcion);
        //}
    }
}