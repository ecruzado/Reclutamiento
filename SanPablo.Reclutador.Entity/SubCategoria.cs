using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class SubCategoria : BaseEntity
    {
        public virtual int IDESUBCATEGORIA { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual int ORDENIMPRESION { get; set; }
        public virtual string NOMSUBCATEGORIA { get; set; }
        public virtual string DESCSUBCATEGORIA { get; set; }
        public virtual string ESTACTIVO { get; set; }
        public virtual string USRCREACION { get; set; }
        public virtual DateTime FECCREACION { get; set; }
        public virtual string USRMODIFICACION { get; set; }
        public virtual DateTime FECMODIFICACION { get; set; }
        public virtual IList<Criterio> Criterios { get; set; }
        public virtual int TIEMPO { get; set; }

        


        
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