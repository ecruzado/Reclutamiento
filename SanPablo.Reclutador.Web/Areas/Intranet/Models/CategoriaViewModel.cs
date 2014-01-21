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

    public class CategoriaViewModel
    {
        public Criterio Criterio { get; set; }
        public Categoria Categoria { get; set; }
        public virtual string IndVisual { get; set; }
        public Alternativa Alternativa { get; set; }
        public virtual List<DetalleGeneral> TipoCategoria { get; set; }
       
    }
}