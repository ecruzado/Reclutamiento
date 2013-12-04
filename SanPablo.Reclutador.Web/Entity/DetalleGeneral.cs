            using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class DetalleGeneral : BaseEntity
    {
        public virtual string CodigoDetalleGeneral { get; set; }
        public virtual General General { get; set; }
        public virtual DetalleGeneral DetallePadre { get; set; }
        public virtual string NombreDetalleGeneral { get; set; }
        public virtual int LongitudCampo { get; set; }
        public virtual string TipoCampo { get; set; }
        public virtual string Abreviatura { get; set; }
        public virtual string Simbolo { get; set; }
        public virtual string EstadoRegistro { get; set; }
   }
}