using SanPablo.Reclutador.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class ExperienciaPostulante
    {
        public virtual int IdeExperienciaPostulante { get; set; }
        public virtual Persona Postulante { get; set; }
        public virtual int IdePostulante { get; set; }
        public virtual string NombreEmpresa { get; set; }
        public virtual string TipoCargoTrabajo { get; set; }
        public virtual DateTime FechaTrabajoInicio { get; set; }
        public virtual DateTime FechaTrabajoFin { get; set; }
        public virtual string IndicadorActualmenteTrabajo { get; set; }
        public virtual string TiempoDeServicio { get; set; }
        public virtual string TipoMotivoCese { get; set; }
        public virtual string NombreReferente { get; set; }
        public virtual int NumeroMovilReferencia { get; set; }
        public virtual string TipoCargoTrabajoReferente { get; set; }
        public virtual int NumeroFijoInstitucionReferente { get; set; }
        public virtual int NumeroAnexoInstitucionReferente { get; set; }
    }
}
