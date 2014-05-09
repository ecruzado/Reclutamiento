using SanPablo.Reclutador.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ConocimientoGeneralCargo : BaseEntity
    {
        public virtual int IdeConocimientoGeneralCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual string TipoConocimientoOfimatica { get; set; }
        public virtual string TipoNombreOfimatica { get; set; }
        public virtual string TipoIdioma { get; set; }
        public virtual string TipoConocimientoIdioma { get; set; }
        public virtual string TipoConocimientoGeneral {get; set; }
        public virtual string TipoNombreConocimientoGeneral { get; set; }
        public virtual string NombreConocimientoGeneral { get; set; }
        public virtual string TipoNivelConocimiento { get; set; }
        public virtual string IndicadorCertificacion { get; set; }
        public virtual int? PuntajeConocimiento { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionConocimientoIdioma { get; set; }
        public virtual string DescripcionConocimientoOfimatica { get; set; }
        public virtual string DescripcionIdioma { get; set; }
        public virtual string DescripcionNivelConocimiento { get; set; }
        public virtual string DescripcionNombreConocimientoGeneral { get; set; }
        public virtual string DescripcionNombreOfimatica { get; set; }
        public virtual string DescripcionConocimientoGeneral { get; set; }
    }
}
