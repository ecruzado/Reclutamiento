namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class EstudioCargoViewModel
    {
        public Cargo Cargo { get; set; }
        public CentroEstudioCargo CentroEstudio { get; set; }
        public NivelAcademicoCargo NivelAcademico { get; set; }

             

    }
}
