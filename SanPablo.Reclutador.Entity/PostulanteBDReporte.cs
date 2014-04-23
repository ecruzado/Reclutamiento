using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;


namespace SanPablo.Reclutador.Entity
{
    
    public class PostulanteBDReporte:BaseEntity
    {
        public virtual int IdePostulante { get; set; }

        public virtual string FechaRegistro { get; set; }
        
        public virtual DateTime? FechaDesde { get; set; }
        
        public virtual DateTime? FechaHasta { get; set; }

        public virtual string Departamento { get; set; }
        
        public virtual string Provincia { get; set; }
        
        public virtual string Distrito { get; set; }

        public virtual int? IdeDepartamento { get; set; }

        public virtual int? IdeProvincia { get; set; }

        public virtual int? IdeDistrito { get; set; }
        
        public virtual string NombreCompleto { get; set; }
        
        public virtual string TelefonoContacto { get; set; }

        public virtual string Email { get; set; }
        
        public virtual string Cargo { get; set; }
        
        public virtual int? Edad { get; set; }

        public virtual int? EdadInicio { get; set; }

        public virtual int? EdadFin { get; set; }
        
        public virtual string TipoEstudio { get; set; }
        
        public virtual string AreaEstudio { get; set; }
        
        public virtual string RangoSalarial { get; set; }

    }
}