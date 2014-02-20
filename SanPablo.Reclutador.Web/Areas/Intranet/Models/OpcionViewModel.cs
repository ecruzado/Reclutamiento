
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    
    
    public class OpcionViewModel
    {
        public Opcion opcion { get; set; }
        public int IdRoll { get; set; }
        public List<DetalleGeneral> TipoEstado { get; set; }
        public List<DetalleGeneral> TipoMenu{ get; set; }




    }
}