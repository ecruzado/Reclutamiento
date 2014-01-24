namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.Web;

    public class PostulanteGeneralViewModel
    {
        public Postulante Postulante { get; set; }
        public virtual IList<DetalleGeneral> TipoDocumentos { get; set; }
        public virtual IList<DetalleGeneral> Nacionalidad { get; set; }
        public virtual IList<DetalleGeneral> Sexo { get; set; }
        public virtual IList<DetalleGeneral> EstadosCiviles { get; set; }
        public virtual IList<Ubigeo> Departamentos { get; set; }
        public virtual IList<Ubigeo> Provincias { get; set; }
        public virtual IList<Ubigeo> Distritos { get; set; }
        public virtual IList<DetalleGeneral> TipoVias { get; set; }
        public virtual IList<DetalleGeneral> TipoZonas { get; set; }

        public virtual IList<DetalleGeneral> TipoSueldosBrutos { get; set; }
        public virtual IList<DetalleGeneral> TipoDisponibilidadesTrabajos { get; set; }
        public virtual IList<DetalleGeneral> TipoDisponibilidadesHorarios { get; set; }
        public virtual IList<DetalleGeneral> TipoHorarios { get; set; }
        public virtual IList<DetalleGeneral> TipoParientesSedes { get; set; }

        public IList<EstudioPostulante> estudiosPostulante { get; set; }

        public HttpPostedFileBase FotoPostulante { get; set; }



    }
}