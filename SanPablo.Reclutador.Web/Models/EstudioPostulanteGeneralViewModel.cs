namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class EstudioPostulanteGeneralViewModel
    {
        public EstudioPostulante Estudio { get; set; }
        public int porcentaje { get; set; }       
        public virtual IList<DetalleGeneral> TipoTipoInstituciones { get; set; }
        public virtual IList<DetalleGeneral> TipoNombreInstituciones { get; set; }
        public virtual IList<DetalleGeneral> AreasEstudio { get; set; }
        public virtual IList<DetalleGeneral> TiposEducacion { get; set; }
        public virtual IList<DetalleGeneral> NivelesAlcanzados { get; set; }
    }
}