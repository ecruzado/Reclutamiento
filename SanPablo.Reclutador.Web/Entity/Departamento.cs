using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Departamento : BaseEntity
    {
        public virtual int CodigoDepartamento { get; set; }
        public virtual int CodigoDependencia { get; set; }
        public virtual string NombreDepartamento { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}