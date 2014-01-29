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

    public class ExamenViewModel
    {
        public Categoria Categoria { get; set; }
        public SubCategoria SubCategoria { get; set; }
        public Criterio Criterio { get; set; }
        public Examen Examen { get; set; }
        public List<DetalleGeneral> TipoEstado { get; set; }
        public List<DetalleGeneral> TipoExamen { get; set; }
        


    }
}