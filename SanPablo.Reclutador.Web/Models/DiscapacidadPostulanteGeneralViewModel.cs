namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class DiscapacidadPostulanteGeneralViewModel
    {
        public  DiscapacidadPostulante  Discapacidad { get; set; }
        public int porcentaje { get; set; }       
        public virtual IList<DetalleGeneral> TipoDiscapacidades { get; set; }
    }
}
