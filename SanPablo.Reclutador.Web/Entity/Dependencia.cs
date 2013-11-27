using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Dependencia : BaseEntity
    {
        public virtual int CodigoDependencia { get; set; }
        public virtual int CodigoSede { get; set; }
        public virtual string NombreDependencia { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}