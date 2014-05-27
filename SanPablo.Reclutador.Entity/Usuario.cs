using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Usuario : BaseEntity
    {
        //public virtual int CodigoUsuario { get; set; }
        //public virtual Rol Rol { get; set; }
        //public virtual Empleado Empleado { get; set; }
        public virtual string NombreUsuario {  get; set; }
        public virtual string ClaveUsuario { get; set; }
        //public virtual string EstadoRegistro { get; set; }
        //public virtual IList<Sede> UsuarioDeLasSedes { get; protected set; }

        //public Usuario()
        //{
        //    UsuarioDeLasSedes = new List<Sede>();
        //}
        
        public virtual int IdUsuario { get; set; }
        public virtual string CodContrasena { get; set; }
        public virtual string CodUsuario { get; set; }
        public virtual string DscApePaterno { get; set; }
        public virtual string DscApeMaterno { get; set; }
        public virtual string DscNombres { get; set; }
        public virtual string Email { get; set; }
        public virtual string Telefono { get; set; }
        public virtual string FlgEstado { get; set; }
        public virtual string UsrCreacion { get; set; }
        public virtual DateTime FecCreacion { get; set; }
        public virtual string UsrModificacion { get; set; }
        public virtual DateTime FecModifcacion { get; set; }
        public virtual string TipUsuario { get; set; }
        public virtual int IdePostulante { get; set; }
        public virtual string IndicadorPostulante { get; set; }

        public virtual List<String> listaSend { get; set; }
        public virtual List<String> listaCopy { get; set; }

        public virtual int IdRol { get; set; }
        public virtual string Rol { get; set; }
       
        
        
    }
}