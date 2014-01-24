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
        public virtual Postulante Postulante { get; set; }
        public virtual int IdePostulante { get; set; }
        public virtual string NombreEmpresa { get; set; }
        public virtual string TipoCargoTrabajo { get; set; }
        public virtual string NombreCargoTrabajo { get; set; }
        public virtual DateTime FechaTrabajoInicio { get; set; }
        public virtual DateTime FechaTrabajoFin { get; set; }
        public virtual string IndicadorActualmenteTrabajo { get; set; }
        public virtual string TiempoDeServicio { get; set; }
        public virtual string TipoMotivoCese { get; set; }
        public virtual string NombreReferente { get; set; }
        public virtual string CorreoReferente { get; set; }
        public virtual int NumeroMovilReferencia { get; set; }
        public virtual string TipoCargoTrabajoReferente { get; set; }
        public virtual int NumeroFijoInstitucionReferente { get; set; }
        public virtual int NumeroAnexoInstitucionReferente { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual bool ActualmenteTrabajando
        {
            get
            {
                return IndicadorActualmenteTrabajo == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    IndicadorActualmenteTrabajo = Indicador.Si;
                else
                    IndicadorActualmenteTrabajo = Indicador.No;
            }
        }
    }
}
