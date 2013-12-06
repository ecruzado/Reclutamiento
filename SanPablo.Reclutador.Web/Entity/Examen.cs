using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity
{
    public class Examen : BaseEntity
    {
        public virtual int CodigoExamen { get; set; }
        public virtual int CodigoSede { get; set; }
        public virtual string NombreExamen { get; set; }
        public virtual string DescripcionExamen { get; set; }
        public virtual string TipoExamen { get; set; }
        public virtual string EstadoRegistro { get; set; }
    }
}