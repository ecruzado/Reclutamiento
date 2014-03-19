
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

    }
}