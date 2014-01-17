namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class ParientePostulanteGeneralViewModel
    {
        public  ParientePostulante  Pariente { get; set; }
        public virtual IList<DetalleGeneral> TipoVinculos { get; set; }
    }
}
