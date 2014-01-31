namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class ExperienciaPostulanteGeneralViewModel
    {
        public ExperienciaPostulante Experiencia { get; set; }
        public int porcentaje { get; set; }       
        public virtual IList<DetalleGeneral> TipoCargos { get; set; }
        public virtual IList<DetalleGeneral> TipoMotivosCese { get; set; }
        public virtual IList<DetalleGeneral> TipoCargosReferente { get; set; }
    }
}