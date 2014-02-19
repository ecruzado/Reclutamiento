
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
        public string Area { get; set; }
        public string Dependencia { get; set; }
        public string Departamento { get; set; }

    }
}
