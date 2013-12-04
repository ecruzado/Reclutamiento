using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Rol : BaseEntity
    {
        public virtual int CodigoRol { get; set; }
        public virtual string NombreRol { get; set; }
        public virtual string EstadoRegistro { get; set; }
        public virtual IList<Opcion> Opciones { get; set; }
       
        public Rol()
        {
            Opciones = new List<Opcion>();
        }
        public virtual void AgregarOpcion(Opcion opcion)
        {
            opcion.Roles.Add(this);
            Opciones.Add(opcion);
        }
    }
}