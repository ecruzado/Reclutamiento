namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class EvaluacionCargoViewModel
    {
        public Cargo Cargo { get; set; }
        public Examen Examen { get; set; }
        public EvaluacionCargo Evaluacion { get; set; }
        public string descExamen { get; set; }
        
        public List<DetalleGeneral> TiposAreasResponsables { get; set; }
        public List<Examen> Examenes { get; set; }
        public List<Examen> TipoExamenes { get; set; }
        

    }
}
