using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Departamento : BaseEntity
    {
        public virtual int IdeDepartamento { get; set; }
        public virtual Dependencia Dependencia { get; set; }
        public virtual string NombreDepartamento { get; set; }
        public virtual string EstadoActivo { get; set; }
    }
}