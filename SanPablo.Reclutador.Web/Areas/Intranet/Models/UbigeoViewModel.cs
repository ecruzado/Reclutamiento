namespace SanPablo.Reclutador.Web.Areas.Intranet.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;
    
    public class UbigeoViewModel
    {
        public UbigeoCargo Ubigeo { get; set; }

        public List<Ubigeo> Departamentos { get; set; }
        public List<Ubigeo> Provincias { get; set; }
        public List<Ubigeo> Distritos { get; set; }

    }
}