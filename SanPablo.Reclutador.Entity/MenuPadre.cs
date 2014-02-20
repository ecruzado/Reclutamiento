using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
   
    public class MenuPadre : BaseEntity
    {

        public virtual int IDOPCIONPADRE { get; set; }
        public virtual string DESCRIPCION { get; set; }
        public virtual string TIPMENU { get; set; }
        
    }
}
