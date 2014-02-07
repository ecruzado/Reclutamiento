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

    public class UbigeoCargoViewModel
    {
        public Cargo Cargo { get; set; }
        public UbigeoCargo Ubigeo { get; set; }
        
        
        public List<Ubigeo> Departamentos { get; set; }
        public List<Ubigeo> Provincias { get; set; }
        public List<Ubigeo> Distritos { get; set; }
        
       

    }
}
