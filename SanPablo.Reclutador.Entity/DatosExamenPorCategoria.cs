using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class DatosExamenPorCategoria : BaseEntity
    {
        public virtual int IdeReclutamientoPersonaExamenCategoria { get; set; }
        public virtual int IdeExamenCategoria { get; set; }
        public virtual int IdeExamen { get; set; }
        public virtual int IdeCategoria { get; set; }
        public virtual string NombreExamen { get; set; }
        
        public virtual string DescripcionExamen { get; set; }
        public virtual string NombreCategoria { get; set; } 
        public virtual int OrdenImpresion { get; set; }
        public virtual int Tiempo { get; set; }
        public virtual string Estado { get; set; }

    }
}