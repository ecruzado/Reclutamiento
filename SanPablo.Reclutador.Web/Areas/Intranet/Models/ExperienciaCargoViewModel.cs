
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class ExperienciaCargoViewModel
    {
        public Cargo Cargo { get; set; }
        public ExperienciaCargo Experiencia { get; set; }
        
        public List<DetalleGeneral> TiposCargo { get; set; }
        
        
    }
}
