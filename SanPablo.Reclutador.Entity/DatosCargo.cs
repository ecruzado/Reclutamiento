using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    
    public class DatosCargo : BaseEntity
    {
        public virtual int IdeCargo { get; set; }
        public virtual string CodigoCargo { get; set; }
        public virtual string NombreCargo { get; set; }
        public virtual string DescripcionCargo { get; set; }
        public virtual int NumeroPosiciones { get; set; }
        public virtual int IdeArea { get; set; }
        public virtual string Area { get; set; }
        public virtual int IdeDependencia { get; set; }
        public virtual string Dependencia { get; set; }
        public virtual int IdeDepartamento { get; set; }
        public virtual string Departamento { get; set; }
        public virtual int IdeSolicitud { get; set; }
        public virtual string TipoEtapa { get; set; }
        public virtual string Pagina { get; set; }
        
    }
}