using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    public class Reemplazo : BaseEntity
    {
        public virtual int IdReemplazo    { get; set; }
        public virtual string ApePaterno  { get; set; }
        public virtual string ApeMaterno  { get; set; }
        public virtual string Nombres     { get; set; }
        public virtual DateTime FecInicioReemplazo  { get; set; }
        public virtual DateTime FecFinReemplazo     { get; set; }
  
    }

}
