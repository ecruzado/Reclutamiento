using SanPablo.Reclutador.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class NivelAcademicoCargo
    {
        public virtual int IdeNivelAcademicoCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual string TipoNivelEstudios { get; set; }
        public virtual string TipoCarrera { get; set; }
        public virtual string TipoNivelAlcanzado { get; set; }
        public virtual int PuntajeNivelEstudio { get; set; }
        public virtual string EstadoACtivo { get; set; }

    }
}
