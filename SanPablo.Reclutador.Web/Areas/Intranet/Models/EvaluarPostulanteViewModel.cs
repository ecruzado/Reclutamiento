namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;
    using System.ComponentModel.DataAnnotations;

    public class EvaluarPostulanteViewModel
    {
        public SubCategoria SubCategoria { get; set; }
        public Categoria Categoria { get; set; }
        public Criterio Criterio { get; set; }

        public List<Alternativa> Alternativas { get; set; }
        public Alternativa Alternativa { get; set; }

        //[DataType(DataType.Time)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")] 
        public string Inicio { get; set; }

        //[DataType(DataType.Time)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")] 
        public string Fin { get; set; }

        public int nroPregunta { get; set; }

        public int totalPreguntas { get; set; }
        

    }
}
