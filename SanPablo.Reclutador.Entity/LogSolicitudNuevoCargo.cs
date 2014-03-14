using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    
    public class LogSolicitudNuevoCargo 
    {
        public virtual int IdeLogSolicitudNuevoCargo { get; set; }
        public virtual int IdeSolicitudNuevoCargo { get; set; }
        public virtual string TipoEtapa { get; set; }
        public virtual int RolResponsable { get; set; }
        public virtual int UsuarioResponsable { get; set; }
        public virtual string Observacion { get; set; }
  
        public virtual DateTime FechaSuceso { get; set; }
        public virtual int UsuarioSuceso { get; set; }
        public virtual int RolSuceso { get; set; }




    }
}