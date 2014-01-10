            using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class DetalleGeneral : BaseEntity
    {
        public virtual string IdeDetalleGeneral { get; set; }
        public virtual General General { get; set; }
        public virtual string TipoTabla { get; set; }
        public virtual string Valor { get; set; }
        public virtual string Descripcion { get; set; }
        
   }
}