using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class General : BaseEntity
    {
        public virtual int IdeGeneral { get; set; }
        public virtual string TipoTabla { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string TipoDato { get; set; }
        public virtual string LongitudCampo { get; set; }
    }
}