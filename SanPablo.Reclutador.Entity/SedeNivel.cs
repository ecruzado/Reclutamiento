

namespace SanPablo.Reclutador.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    
   
    public class SedeNivel : BaseEntity
    {
         public virtual int IDUSUARIONIVEL { get; set; }
         public virtual int IDUSUARIO { get; set; }
         public virtual int IDESEDE { get; set; }
         public virtual int IDEDEPARTAMENTO { get; set; }
         public virtual int IDEDEPENDENCIA { get; set; }
         public virtual int IDEAREA { get; set; }
         public virtual string FLGESTADO { get; set; }
         public virtual string SEDEDES { get; set; }
        
         public virtual string DEPENDENCIADES { get; set; }
         public virtual string DEPARTAMENTODES { get; set; }
         public virtual string AREADES { get; set; }

      

    }
}
