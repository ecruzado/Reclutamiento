using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class NotificacionDeAprobacion : BaseEntity
    {
        public virtual int CodigoNotificaionDeAprobacion { get; set; }
        public virtual int CodigoConfiguracionDeAprobacion { get; set; }
        public virtual string CodigoProceso { get; set; }
        public virtual int NumeroSuceso { get; set; }
        public virtual int CodigoCargo { get; set; } 
    }
}