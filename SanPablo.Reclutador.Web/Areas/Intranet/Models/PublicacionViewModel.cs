
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class PublicacionViewModel
    {
        public Cargo Cargo { get; set; }
        public SolicitudNuevoCargo SolicitudCargo { get; set; }
        public string Area { get; set; }
        public string Sede { get; set; }
        public string TipoHorario { get; set; }
        public string RangoSalario { get; set; }

        public string btnPublicar { get; set; }
        public string btnActualizar { get; set; }


        public string pagina { get; set; }

        public string visualizarCompetencias { get; set; }
        public string visualizarOfrecemos { get; set; }
        public string visualizarDiscapacidad { get; set; }

        public string editarObservaciones { get; set; }
        public string editarFechaPublicacion { get; set; }
        public string editarFechaExpiracion { get; set; }
    }
}
