using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class UbigeoCargo : BaseEntity
    {
        public virtual int IdeUbigeoCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual int IdeUbigeo { get; set; }
        public virtual int? PuntajeUbigeo { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual string Departamento { get; set; }
        public virtual string Provincia { get; set; }
        public virtual string Distrito { get; set; }


        public virtual int IdeDepartamento { get; set; }
        public virtual int IdeProvincia { get; set; }
    }
}