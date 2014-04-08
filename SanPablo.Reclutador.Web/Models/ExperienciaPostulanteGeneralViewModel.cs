namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class ExperienciaPostulanteGeneralViewModel
    {
        public ExperienciaPostulante Experiencia { get; set; }
        public int porcentaje { get; set; }       
        public IList<DetalleGeneral> TipoCargos { get; set; }
        public IList<DetalleGeneral> TipoMotivosCese { get; set; }
        public IList<DetalleGeneral> TipoCargosReferente { get; set; }
    }
}