namespace SanPablo.Reclutador.Web.Models
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public class UbigeoGeneralViewModel
    {
        public Ubigeo Ubigeo { get; set; }
        public virtual IList<ItemTabla> Departamentos { get; set; }
        public virtual IList<ItemTabla> Provincias { get; set; }
        public virtual IList<ItemTabla> Distritos { get; set; }
       


    }
}