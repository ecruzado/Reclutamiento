namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class EstudioPostulanteGeneralViewModel
    {
        public EstudioPostulante Estudio { get; set; }
        public virtual IList<ItemTabla> TipoInstituciones { get; set; }
        public virtual IList<ItemTabla> Instituciones { get; set; }
        public virtual IList<ItemTabla> AreasEstudio { get; set; }
        public virtual IList<ItemTabla> Educacion { get; set; }
        public virtual IList<ItemTabla> NivelesAlcanzados { get; set; }
    }
}