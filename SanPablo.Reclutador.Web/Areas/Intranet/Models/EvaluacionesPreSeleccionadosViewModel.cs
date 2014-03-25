
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{

    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    
    public class EvaluacionesPreSeleccionadosViewModel
    {
        public Postulante PostulantePreSel { get; set; }
        public SolReqPersonal Solicitud { get; set; }
        public ReclutamientoPersona ReclutaPersona { get; set; }
        public ReclutamientoPersonaExamen ReclutaPersonaExamen { get; set; }

        public List<DetalleGeneral> listaEstaPost { get; set; }

        public string nombreUsuario { get; set; }
        public string pagina { get; set; }

        public HttpPostedFileBase ArchivoComentario { get; set; }
        public string nombreTemporalArchivo { get; set; }
        
        public List<DetalleGeneral> ListaAprobadoDesaprobado { get; set; }
        public string tipoArchivo { get; set; }

    }
}