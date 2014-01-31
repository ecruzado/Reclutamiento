namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class ConocimientoPostulanteGeneralViewModel
    {
        public  ConocimientoGeneralPostulante  ConocimientoGeneral { get; set; }

        public int porcentaje { get; set; }       
        public virtual IList<DetalleGeneral> TiposConocimientoOfimatica { get; set; }
        public virtual IList<DetalleGeneral> TipoNombresOfimatica { get; set; }
        public virtual IList<DetalleGeneral> TipoNivelesConocimiento { get; set; }
        public virtual IList<DetalleGeneral> TipoIdiomas { get; set; }
        public virtual IList<DetalleGeneral> TipoConocimientoIdiomas { get; set; }
        public virtual IList<DetalleGeneral> TipoConocimientoGenerales { get; set; }
        public virtual IList<DetalleGeneral> TipoNombresConocimientosGrales { get; set; }
        
    }
}

