using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Sede : BaseEntity
    {
        public virtual string CodigoSede { get; set; }
        public virtual string DescripcionSede { get; set; }
        public virtual string EstadoRegistro { get; set; }
        public virtual IList<Usuario> Usuarios { get; protected set; }
        public virtual IList<Empleado> Empleados { get; protected set; }

        public Sede()
        {
            Usuarios = new List<Usuario>();
            Empleados = new List<Empleado>();
        }
        public virtual void AgregarUsuario(Usuario usuario)
        {
            usuario.UsuarioDeLasSedes.Add(this);
            Usuarios.Add(usuario);
        }
        public virtual void AgregarEmpleado(Empleado empleado)
        {
            empleado.Sede = this;
            Empleados.Add(empleado);
        }
    }
}