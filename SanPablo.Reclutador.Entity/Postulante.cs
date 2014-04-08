using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;


namespace SanPablo.Reclutador.Entity
{
    
    public class Postulante:BaseEntity
    {
        public virtual int IdePostulante { get; set; }
        public virtual string TipoDocumento { get; set; }
        public virtual string NumeroDocumento { get; set; }
        public virtual string ApellidoPaterno { get; set; }
        public virtual string ApellidoMaterno { get; set; }
        public virtual string PrimerNombre { get; set; }
        public virtual string SegundoNombre { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FechaNacimiento { get; set; }

        public virtual string NumeroLicencia { get; set; }
        public virtual string IndicadorSexo { get; set; }
        public virtual string TipoEstadoCivil { get; set; }
        public virtual string TipoVia { get; set; }
        public virtual string NombreVia { get; set; }
        public virtual int? NumeroDireccion { get; set; }
        public virtual string InteriorDireccion { get; set; }
        public virtual string Manzana { get; set; }
        public virtual string Lote { get; set; }
        public virtual string Bloque { get; set; }
        public virtual string Etapa { get; set; }

        public virtual string Correo { get; set; }
        public virtual string Observacion { get; set; }

        [DataType(DataType.PhoneNumber)]
        public virtual int? TelefonoMovil { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        //

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2,3})\)?([-]{1})([0-9]{3})([-]{1})([0-9]{3,4})$", ErrorMessage = "Formato de telefono no válido : (054)-124-1245")]
        public virtual string TelefonoFijo { get; set; }
        public virtual string TipoZona { get; set; }
        public virtual string NombreZona { get; set; }
        public virtual int IdeUbigeo { get; set; }
        public virtual string TipoNacionalidad { get; set; }
        public virtual string ReferenciaDireccion { get; set; }
       
        public virtual IList<EstudioPostulante> Estudios { get; set; }
        public virtual IList<ExperienciaPostulante> Experiencias { get; set; }
        public virtual IList<ConocimientoGeneralPostulante> Conocimientos { get; set; }
        public virtual IList<ParientePostulante> Parientes { get; set; }
        public virtual IList<DiscapacidadPostulante> Discapacidades { get; set; }

        public virtual string TipoSalario { get; set; }
        public virtual string TipoDisponibilidadTrabajo { get; set; }
        public virtual string TipoDisponibilidadHorario { get; set; }
        public virtual string TipoHorario { get; set;}
        public virtual string IndicadorReubicarseInterior { get; set; }
        public virtual string IndicadorParientesCHSP { get; set; }
        public virtual string TipoParienteSede { get; set; }
        public virtual string ParienteNombre { get; set; }
        public virtual string ParienteCargo { get; set; }
        public virtual string DescripcionOtroMedio { get; set; }
        public virtual string TipoComoSeEntero { get; set; }

        /// <summary>
        /// tipo de puesto
        /// </summary>
        public virtual string TipoPuesto { get; set; }
        /// <summary>
        /// cargo a la que postula
        /// </summary>
        public virtual int IdCargo { get; set; }
        /// <summary>
        /// sede a la que postula
        /// </summary>
        public virtual int IdSede { get; set; }

        /// <summary>
        /// Nombre Completo del postulante
        /// </summary>
        public virtual string NombreCompleto {
            get 
            {
                return ApellidoPaterno + " " + ApellidoMaterno + " " + PrimerNombre + " " + SegundoNombre; 
            }
        }

        public virtual byte[] FotoPostulante { get; set; }

        public virtual string IndicadorRegistroCompleto { get; set; }

        public virtual string EstadoActivo { get; set; }

        public Postulante()
        {
            Estudios = new List<EstudioPostulante>();
            Experiencias = new List<ExperienciaPostulante>();
            Conocimientos = new List<ConocimientoGeneralPostulante>();
            Parientes = new List<ParientePostulante>();
            Discapacidades = new List<DiscapacidadPostulante>();
            
        }

        public virtual void agregarEstudio(EstudioPostulante estudioPostulante)
        {
            estudioPostulante.Postulante = this;
            Estudios.Add(estudioPostulante);
        }

        public virtual void agregarExperiencia(ExperienciaPostulante experienciaPostulante)
        {
            experienciaPostulante.Postulante = this;
            Experiencias.Add(experienciaPostulante);
        }
        
        public virtual void agregarConocimiento(ConocimientoGeneralPostulante conocimientoPostulante)
        {
            conocimientoPostulante.Postulante = this;
            Conocimientos.Add(conocimientoPostulante);
        }

        public virtual void agregarPariente(ParientePostulante parientePostulante)
        {
            parientePostulante.Postulante = this;
            Parientes.Add(parientePostulante);
        }

        public virtual void agregarDiscapacidad(DiscapacidadPostulante discapacidadPostulante)
        {
            discapacidadPostulante.Postulante = this;
            Discapacidades.Add(discapacidadPostulante);
        }

        private bool beavaliddate(DateTime date)
        {

            DateTime fechaValida = DateTime.Now.AddYears(-18);

            if (date < fechaValida)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /*-------------CvPostulante----------------------*/
        /// <summary>
        /// lista de postulantes para el Cv del postulante
        /// </summary>
        public virtual DataTable CurPostulante  { get; set; }
        /// <summary>
        /// lista de nivel academico para el Cv del postulante
        /// </summary>
        public virtual DataTable CurNivelAcademico { get; set; }
        /// <summary>
        /// lista de experiencias para el Cv del postulante
        /// </summary>
        public virtual DataTable CurExperiencia { get; set; }
        /// <summary>
        /// lista de conocimiento de ofimatico para el Cv del postulante
        /// </summary>
        public virtual DataTable CurConOfimatica { get; set; }
        /// <summary>
        /// lista de conocimientos de Idiomas para el Cv del postulante
        /// </summary>
        public virtual DataTable CurConIdioma { get; set; }
        /// <summary>
        /// lista de otros conocimientos para el postulante
        /// </summary>
        public virtual DataTable CurOtrosCon { get; set; }
        /// <summary>
        /// lista de parientes para el cv del postulante
        /// </summary>
        public virtual DataTable CurParientes { get; set; }
        /// <summary>
        /// lista de discapacidad para el cv del postulante
        /// </summary>
        public virtual DataTable CurDiscapacidad { get; set; }
        /*-------------CvPostulante----------------------*/

    }
}