namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class PostulanteGeneralViewModel
    {
        public Persona Persona { get; set; }
        public virtual IList<ItemTabla> TipoDocumentos { get; set; }
        public virtual IList<ItemTabla> Nacionalidad { get; set; }
        public virtual IList<ItemTabla> Sexo { get; set; }
        public virtual IList<ItemTabla> EstadosCiviles { get; set; }
        public virtual IList<ItemTabla> Departamentos { get; set; }

        public IList<EstudioPostulante> estudiosPostulante { get; set; }



    }
}