using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Ubigeo : BaseEntity
    {
        public virtual int CodigoUbigeo { get; set; }
        public virtual int NombreUbigeo { get; set; }
        public virtual int CodigoUbigeoPadre { get; set; }
        public virtual string Codigo { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}