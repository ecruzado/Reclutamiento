using SanPablo.Reclutador.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
   
    public class Edad : BaseEntity
    {
        public virtual int IdEdad { get; set; }
        public virtual string DesEdad { get; set; }
    }
}
