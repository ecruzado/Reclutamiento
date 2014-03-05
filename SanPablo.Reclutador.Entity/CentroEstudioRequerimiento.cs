using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class CentroEstudioRequerimiento : BaseEntity
    {
        public virtual int IdeCentroEstudioRequerimiento { get; set; }
        public virtual SolReqPersonal SolicitudRequerimiento { get; set; }
        public virtual string TipoCentroEstudio { get; set; }
        public virtual string TipoNombreCentroEstudio { get; set; }
        public virtual int PuntajeCentroEstudios { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionTipoCentroEstudio { get; set; }
        public virtual string DescripcionNombreCentroEstudio { get; set; }
        
    }
}
