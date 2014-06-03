namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;


    public class ReportePostulantesPotencialesViewModel
    {
        public ReportePostulantePotencial PostulantePotencial { get; set; }

        public List<Cargo> Cargos { get; set; }

        public List<DetalleGeneral> AreasEstudio { get; set; }

        public List<DetalleGeneral> RangosSalariales { get; set; }

        public List<Sede> Sedes { get; set; }
        
        public List<Dependencia> Dependencias { get; set; }

        public List<Departamento> Departamentos { get; set; }

        public List<Area> Areas { get; set; }

        public string btnVerReporte { get; set; }

        public List<Edad> ListaEdadInicio { get; set; }

        public List<Edad> ListaEdadFin { get; set; }

    }
}