using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class General : BaseEntity
    {
        public virtual string CodigoGeneral { get; set; }
        public virtual string NombreGeneral { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}