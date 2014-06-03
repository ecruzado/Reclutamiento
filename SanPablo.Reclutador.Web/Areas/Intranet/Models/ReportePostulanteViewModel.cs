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


    public class ReportePostulanteViewModel
    {
        public SolReqPersonal Solicitud { get; set; }

        public Reporte ReporteSol { get; set; }

        public List<DetalleGeneral> AreasEstudio { get; set; }

        public DetalleGeneral AreaEstudio { get; set; }

        public List<DetalleGeneral> RangosSalariales { get; set; }

        public DetalleGeneral RangoSalarial { get; set; }

        public List<Ubigeo> Departamentos { get; set; }

        public Ubigeo Departamento { get; set; }
        
        public List<Ubigeo> Provincias { get; set; }

        public Ubigeo Provincia { get; set; }

        public List<Ubigeo> Distritos { get; set; }

        public Ubigeo Distrito { get; set; }

        public int? EdadInicio { get; set; }

        public int? EdadFin { get; set; }

        public string btnVerReporte { get; set; }


        public List<Edad> listaEdad { get; set; }

    }
}