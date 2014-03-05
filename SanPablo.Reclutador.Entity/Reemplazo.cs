using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web; 


namespace SanPablo.Reclutador.Entity
{
    public class Reemplazo : BaseEntity
    {
        public virtual int IdReemplazo    { get; set; }
        public virtual int Indicador { get; set; }
        public virtual string ApePaterno  { get; set; }
        public virtual string ApeMaterno  { get; set; }
        public virtual string Nombres     { get; set; }

        public virtual string CodGenerado { get; set; }

        public virtual string indEdcicion { get; set; }

        public virtual int IdeSolReqPersonal    { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FecFinalReemplazo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FecInicioReemplazo  { get; set; }

       
    }

}
