using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Entity
{
    public class Persona
    {
        public virtual int IdePersona { get; set; }
        public virtual string TipoDocumento { get; set; }
        public virtual string NumeroDocumento { get; set; }
        public virtual string ApellidoPaterno { get; set; }
        public virtual string ApellidoMaterno { get; set; }
        public virtual string PrimerNombre { get; set; }
        public virtual string SegundoNombre { get; set; }
        public virtual DateTime FechaNacimiento { get; set; }
        public virtual string NumeroLicencia { get; set; }
        public virtual string IndicadorSexo { get; set; }
        public virtual string TipoEstadoCivil { get; set; }
        public virtual string Observacion { get; set; }

        public virtual IList<ItemTabla> TipoDocumentos { get; set; }
        public virtual IList<ItemTabla> Nacionalidad { get; set; }
        public virtual IList<ItemTabla> Sexo { get; set; }
        public virtual IList<ItemTabla> EstadosCiviles { get; set; }
    }
}