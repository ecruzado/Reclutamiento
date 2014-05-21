

namespace SanPablo.Reclutador.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web; 
    
    public class CvPostulante : BaseEntity
    {
        /// <summary>
        /// id del postulante
        /// </summary>
        public virtual int IdCvPostulante { get; set; }
        /// <summary>
        /// Apellido paterno del postulante
        /// </summary>
        public virtual string ApePaterno { get; set; }

        /// <summary>
        /// Apellido materno del postulante
        /// </summary>
        public virtual string ApeMaterno { get; set; }

        /// <summary>
        /// Dni del postulante
        /// </summary>
        public virtual string Dni { get; set; }
        /// <summary>
        /// numero de telefono
        /// </summary>
        public virtual string Telefono { get; set; }
        

        /// <summary>
        /// Fecha de cita del postulante
        /// </summary>
        // [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? Fechacita { get; set; }

        /// <summary>
        /// hora de cita
        /// </summary>
        //[DataType(DataType.Time)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public virtual DateTime? HoraCita { get; set; }

        /// <summary>
        /// indicador de citado
        /// </summary>
        public virtual bool IndicadorCitado
        {
            get
            {
                return Citado == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    Citado = Indicador.Si;
                else
                    Citado = Indicador.No;
            }
        }
        /// <summary>
        /// Inidicador de asistencia
        /// </summary>
        public virtual bool IndicadorAsistio
        {
            get
            {
                return Asistio == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    Asistio = Indicador.Si;
                else
                    Asistio = Indicador.No;
            }
        }

      

        /// <summary>
        /// indicador de citado S = SI, N = NO
        /// </summary>
        public virtual string Citado { get; set; }

        /// <summary>
        /// indicador de Asitio S = SI asistio, N = NO asistio
        /// </summary>
        public virtual string Asistio { get; set; }

        /// <summary>
        /// indicador de Asitio S = SI asistio, N = NO asistio
        /// </summary>
        public virtual string Nombre { get; set; }

        /// <summary>
        /// id de solicitud
        /// </summary>
        public virtual int IdSolicitud { get; set; }

        /// <summary>
        /// tipo de solicitud
        /// </summary>
         public virtual string TipSol { get; set; }

         public virtual DateTime FecCreacion { get; set; }
         public virtual string UsrCreacion { get; set; }
         public virtual DateTime FecModificacion { get; set; }
         public virtual string UsrModifcacion { get; set; }
        


        ///datos del cv del postulante
        ///
        public virtual string Codtipdocumento { get; set; }
        public virtual string Destipdocumento { get; set; }
        public virtual string Numdocumento { get; set; }
        //public virtual string Apepaterno { get; set; }
        //public virtual string Apematerno { get; set; }
        public virtual string Prinombre { get; set; }
        public virtual string Segnombre { get; set; }
        public virtual string Codnacionalidad { get; set; }
        public virtual string Desnacionalidad { get; set; }
        public virtual string Fecnacimiento { get; set; }
        public virtual string Codsexo { get; set; }
        public virtual string Dessexo { get; set; }
        public virtual string Codestadocivil { get; set; }
        public virtual string Desestadocivil { get; set; }
        public virtual string Numlicencia { get; set; }
        public virtual string Observacion { get; set; }
        public virtual string Pais { get; set; }
        public virtual string Ideubigeo { get; set; }
        public virtual string Desdistrito { get; set; }
        public virtual string Desprovincia { get; set; }
        public virtual string Desdepartamento { get; set; } 
        public virtual string Correo { get; set; }
        public virtual string Telmovil { get; set; }
        public virtual string Telfijo { get; set; }
        public virtual string Referencia { get; set; }
        public virtual string Codtipvia { get; set; }
        public virtual string Destipvia { get; set; }
        public virtual string Nomvia { get; set; }
        public virtual string Numdireccion { get; set; }
        public virtual string Manzana { get; set; }
        public virtual string Bloque { get; set; }
        public virtual string Codtipzona { get; set; }
        public virtual string Destipzona { get; set; }
        public virtual string Nomzona { get; set; }
        public virtual string Interior { get; set; }
        public virtual string Lote { get; set; }
        public virtual string Etapa { get; set; }
        public virtual byte[] Fotopostulante { get; set; }
        public virtual string Salario { get; set; }
        public virtual string Disptrabajo { get; set; }
        public virtual string Dispphorario { get; set; }
        public virtual string Horatrabajo { get; set; }
        public virtual string Reubicacion { get; set; }
        public virtual string Parientetrab { get; set; }
        public virtual string Parientesede { get; set; }
        public virtual string Parientecargo { get; set; }
        public virtual string Comoseentero { get; set; }
        public virtual string Edad { get; set; }
        public virtual string Desedad { get; set; }
        public virtual string Nombrecompleto { get; set; }
        public virtual string Idepostulante { get; set; }
        public virtual string Desdir { get; set; }


        //Experiencias

        public virtual int Ideexppostulante { get; set; }
        public virtual string Nomempresa { get; set; }
        public virtual string Tiemposervicio { get; set; }
        public virtual string Cargo { get; set; }
        public virtual string Fectrabajo { get; set; }
        public virtual string Fucniones { get; set; }
        public virtual string Motivocese { get; set; }
        public virtual string Nomreferente { get; set; }
        public virtual string Fonoinst { get; set; }
        public virtual string Anexoinst { get; set; }
        public virtual string Cargoreferente { get; set; }
        public virtual string Fonoreferente { get; set; }
        public virtual string Correoreferente { get; set; }

    }
}
