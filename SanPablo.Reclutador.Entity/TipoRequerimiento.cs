using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
   
    public class TipoRequerimiento : BaseEntity
    {

        public virtual int IDUSUREQ { get; set; }
        public virtual int IDUSUARIO { get; set; }
        public virtual string TIPREQ { get; set; }
        public virtual string DESREQ { get; set; }
        public virtual string DESUSUARIO { get; set; }
        
        

    }
}
