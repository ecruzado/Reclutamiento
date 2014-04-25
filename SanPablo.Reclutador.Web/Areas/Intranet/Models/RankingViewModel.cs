
namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{

    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    
    public class RankingViewModel
    {
        public CvPostulante CvPostulanteEx { get; set; }
        public SolReqPersonal Solicitud { get; set; }
        public ReclutamientoPersona ReclutaPersonal { get; set; }

        public List<DetalleGeneral> listaEstaPost { get; set; }

        public string pagina { get; set; }
        public string indPagina { get; set; }



        #region

        //acceso de botones

        public string btnAgregar { get; set; }
        public string btnEditar { get; set; }
        public string btnEliminar { get; set; }
        public string btnCitado { get; set; }
        public string btnAsistio { get; set; }
        public string btnBuscar { get; set; }
        public string btnAprobado { get; set; }
        public string btnCvPreseleccion { get; set; }
        

        #endregion

    }
}