﻿using SanPablo.Reclutador.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ParientePostulante
    {
        public virtual int IdeParientesPostulante { get; set; }
        public virtual Persona Postulante { get; set; }
        public virtual int IdePostulante { get; set; }
        public virtual string ApellidoPaterno { get; set; }
        public virtual string ApellidoMaterno { get; set; }
        public virtual string Nombres { get; set; }
        public virtual string TipoDeVinculo { get; set; }
        public virtual DateTime FechaNacimiento { get; set; }
    }
}