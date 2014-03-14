using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPablo.Reclutador.Entity
{
    public class ListaSolicitudNuevoCargo 
    {
       
        public virtual int IdeSolicitudNuevoCargo { get; set; }
        public virtual string EstadoActivo { get; set; }
        public virtual string CodigoCargo { get; set; }
        public virtual string NombreCargo { get; set; }
        public virtual int IdeDependencia { get; set; }
        public virtual string NombreDependencia { get; set; }
        public virtual int IdeDepartamento { get; set; }
        public virtual string NombreDepartamento { get; set; }
        public virtual int IdeArea { get; set; }
        public virtual string NombreArea { get; set; }
        public virtual int NumeroPosiciones { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        public virtual string Responsable { get; set; }
        public virtual int IdeResponsable { get; set; }
        public virtual string NombreResponsable { get; set; }
        public virtual string TipoEtapa { get; set; }
        public virtual string Etapa { get; set; }
        
    }
}
