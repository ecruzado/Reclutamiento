using FluentValidation;
using FluentValidation.Results;
using NHibernate.Criterion;
using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Entity.Validation;
using SanPablo.Reclutador.Repository.Interface;


namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using SanPablo.Reclutador.Entity;

    public class PerfilViewModel
    {
        public Cargo Cargo { get; set; }
        public CompetenciaCargo Competencia { get; set; }
        public OfrecemosCargo Ofrecimiento { get; set; }
        public NivelAcademicoCargo NivelAcademico { get; set; }
        public CentroEstudioCargo CentroEstudio { get; set; }
        public HorarioCargo Horario { get; set; }
        public UbigeoCargo Ubigeo { get; set; }
        public ConocimientoGeneralCargo Conocimiento { get; set; }
        public ExperienciaCargo Experiencia { get; set; }
        public DiscapacidadCargo Discapacidad { get; set; }

        public List<DetalleGeneral> Competencias { get; set; }
        public List<DetalleGeneral> Ofrecimientos { get; set; }
        public List<DetalleGeneral> Sexos { get; set; }
        public List<DetalleGeneral> TiposRequerimientos { get; set; }
        public List<DetalleGeneral> RangoSalariales { get; set; }
        public List<DetalleGeneral> Horarios { get; set; }
        
        public List<Ubigeo> Departamentos { get; set; }
        public List<Ubigeo> Provincias { get; set; }
        public List<Ubigeo> Distritos { get; set; }
        
        public List<DetalleGeneral> TiposEducacion { get; set; }
        public List<DetalleGeneral> AreasEstudio { get; set; }
        public List<DetalleGeneral> NivelesAlcanzados { get; set; }

        public List<DetalleGeneral> TiposInstitucion { get; set; }
        public List<DetalleGeneral> Instituciones { get; set; }

        public List<DetalleGeneral> TipoConocimientos { get; set; }
        public List<DetalleGeneral> DescripcionConocimiento { get; set; }
        public List<DetalleGeneral> NivelesConocimientos { get; set; }

        public List<DetalleGeneral> TipoCargos { get; set; }
        public List<DetalleGeneral> TipoDiscapacidad { get; set; }

    }
}
