﻿namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class CompetenciaCargoViewModel
    {
        public Cargo Cargo { get; set; }
        public CompetenciaCargo Competencia { get; set; }
        
        public List<DetalleGeneral> Competencias { get; set; }
        

    }
}
