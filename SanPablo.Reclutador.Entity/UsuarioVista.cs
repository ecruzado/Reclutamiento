using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPablo.Reclutador.Entity
{
    public class UsuarioVista : BaseEntity
    {
       
        public virtual int IDUSUARIO { get; set; }
         public virtual int IDROL { get; set; }
         public virtual int IDESEDE { get; set; }
        public virtual string DSCNOMBRES { get; set; }
        public virtual string DSCAPEPATERNO { get; set; }
        public virtual string DSCAPEMATERNO { get; set; }
        public virtual string EMAIL { get; set; }
        public virtual string CODUSUARIO { get; set; }
        public virtual string FLGESTADO { get; set; }
        public virtual string DESROL { get; set; }
        public virtual string DESSEDE { get; set; }
        public virtual string TIPUSUARIO { get; set; }
        
    }
}
