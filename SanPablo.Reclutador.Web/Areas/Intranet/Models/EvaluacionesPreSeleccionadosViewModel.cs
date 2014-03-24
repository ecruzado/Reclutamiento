
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

        public List<DetalleGeneral> listaEstaPost { get; set; }

        public string pagina { get; set; }

    }
}