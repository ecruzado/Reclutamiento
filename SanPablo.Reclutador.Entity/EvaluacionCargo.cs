using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class EvaluacionCargo : BaseEntity
    {
        public virtual int IdeEvaluacionCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual Examen Examen { get; set; }
        
        public virtual string TipoExamen { get; set; }
        public virtual int? NotaMinimaExamen { get; set; }
        public virtual string TipoAreaResponsable { get; set; }
        public virtual int PuntajeExamen { get; set; }
        public virtual string EstadoActivo { get; set; }

        public virtual string DescripcionExamen { get; set; }
        public virtual string DescripcionTipoExamen { get; set; }
        public virtual string DescripcionAreaResponsable { get; set; }

        /// <summary>
        /// Indicador de entrevista final
        /// </summary>
        public virtual string IndEntrevFinal { get; set; }
           

        public virtual int IdeExamen
        {
            get
            {
                return Examen == null?0: Examen.IdeExamen;
            }
            set
            {
                if (value != 0)
                {
                    Examen = new Examen();
                    Examen.IdeExamen = value;
                }
               
            }
        }


        /// <summary>
        /// flag para el indicador de entervista final
        /// </summary>
        public virtual bool EsEntrevistaFinal
        {
            get
            {
                return IndEntrevFinal == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    IndEntrevFinal = Indicador.Si;
                else
                    IndEntrevFinal = Indicador.No;
            }
        }

        
    }
}