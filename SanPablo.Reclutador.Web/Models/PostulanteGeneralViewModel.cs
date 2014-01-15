namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class PostulanteGeneralViewModel
    {
        public Persona Persona { get; set; }
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



    }
}