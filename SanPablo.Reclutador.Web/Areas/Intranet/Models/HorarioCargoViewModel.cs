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

    public class HorarioCargoViewModel
    {
        public Cargo Cargo { get; set; }
        public HorarioCargo Horario { get; set; }
        
        public List<DetalleGeneral> Horarios { get; set; }
        
        

    }
}
