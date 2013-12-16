using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class SubCategoria : BaseEntity
    {
        public virtual int CodigoSubCategoria { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual string NombreSubCategoria { get; set; }
        public virtual string DescripcionSubCategoria { get; set; }
        public virtual string EstadoRegistro { get; set; }
        public virtual IList<Criterio> Criterios { get; set; }

        public SubCategoria()
        {
            Criterios = new List<Criterio>();
        }
        public virtual void AgregarCriterios(Criterio criterio)
        {
            criterio.SubCategorias.Add(this);
            Criterios.Add(criterio);
        }
    }
}