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

    public class CargoViewModel
    {
        public Cargo Cargo { get; set; }
        public CompetenciaCargo Competencia { get; set; }
        public NivelAcademicoCargo NivelAcademico { get; set; }
        public CentroEstudioCargo CentroEstudio { get; set; }
        public List<DetalleGeneral> Competencias { get; set; }
    }
}
