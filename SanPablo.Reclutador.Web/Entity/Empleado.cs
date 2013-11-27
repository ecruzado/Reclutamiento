using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Empleado : BaseEntity
    {
        public virtual int CodigoEmpleado { get; set; }
        public virtual int CodigoCargo { get; set; }
        public virtual string TipoDocumentoIdentidad { get; set; }
        public virtual string NumeroDocumentoIdentidad { get; set; }
        public virtual string ApellidoPaterno { get; set; }
        public virtual string ApellidoMaterno { get; set; }
        public virtual string Nombres { get; set; }
        public virtual string Correo { get; set; }
        public virtual int CodigoSede { get; set; }
    }
}