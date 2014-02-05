﻿using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class CentroEstudioCargo
    {
        public virtual int IdeCentroEstudioCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual string TipoCentroEstudio { get; set; }
        public virtual string TipoNombreCentroEstudio { get; set; }
        public virtual string PuntajeCentroEstudios { get; set; }
        public virtual string EstadoActivo { get; set; }
        
    }
}
