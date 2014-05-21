using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    
    public class SolicitudConsulta : BaseEntity
    {
        public virtual int IdeSolicitud { get; set; }
        public virtual string EstadoSolicitud { get; set; }
        public virtual string CodigoSolicitud { get; set; }
        public virtual int IdeCargo { get; set; }
        public virtual string NombreCargo { get; set; }
        public virtual int IdeDependencia { get; set; }
        public virtual string NombreDependencia { get; set; }
        public virtual int IdeDepartamento { get; set; }
        public virtual string NombreDepartamento { get; set; }
        public virtual int IdeArea { get; set; }
        public virtual string NombreArea { get; set; }
        public virtual int NumeroVacantes { get; set; }
        public virtual int Postulantes { get; set; }

        public virtual int Preseleccionados { get; set; }
        public virtual int Evaluados { get; set; }
        public virtual int Seleccionados { get; set; }

        public virtual int Contratados { get; set; }
        
        public virtual DateTime? FechaInicio { get; set; }
        
        public virtual DateTime? FechaCierre { get; set; }
        
        public virtual int  IdeRolResponsable { get; set; }
        
        public virtual string RolResponsable { get; set; }
        
        public virtual string NombreResponsable { get; set; }

        public virtual DateTime? FechaPublicacion { get; set; }

        public virtual DateTime? FechaExpiracion { get; set; }

        public virtual string Publicado { get; set; }
        public virtual string TipoEtapa { get; set; }
        public virtual string Etapa { get; set; }
        public virtual string TipoSolicitud { get; set; }
        public virtual string NombreTipoSolicitud { get; set; }
       
    }
}