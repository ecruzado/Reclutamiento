using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class DetalleGeneral : BaseEntity
    {
        public virtual int IdeGeneral { get; set; }
        public virtual General General { get; set; }
        public virtual string Valor { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string Referencia { get; set; }
        public virtual string IndicadorActivo { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            DetalleGeneral p = obj as DetalleGeneral;
            if ((System.Object)p == null)
            {
                return false;
            }

            return (IdeGeneral == p.IdeGeneral) && (Valor == p.Valor);
        }

        public override int GetHashCode()
        {
            return IdeGeneral * 2;
        }
   }
}