using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace SanPablo.Reclutador.Entity
{
    public class ListaCriterios : BaseEntity
    {
        public virtual List<Criterio> criterios { get; set; }
    }
}