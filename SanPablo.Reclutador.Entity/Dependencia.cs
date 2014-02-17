using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Dependencia : BaseEntity
    {
        public virtual int IdeDependencia { get; set; }
        public virtual int IdeSede { get; set; }
       // public virtual Sede Sede { get; set; }
        public virtual string NombreDependencia { get; set; }
        public virtual string EstadoActivo { get; set; }
    }
}