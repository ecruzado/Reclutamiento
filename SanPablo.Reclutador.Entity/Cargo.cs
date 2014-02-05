using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    
    public class Cargo
    {
        public virtual int IdeCargo { get; set; }
        public virtual int IdeSede { get; set; }
        public virtual string NombreCargo { get; set; }
        public virtual string DescripcionCargo { get; set; }
        public virtual int IdeArea { get; set; }
        public virtual int NumeroPosiciones { get; set; }
        public virtual int PuntajePostulanteInterno { get; set; }
        public virtual string IndicadorSexo { get; set; }
        public virtual string Sexo { get; set; }
        public virtual int EdadInicio { get; set; }
        public virtual int EdadFin { get; set; }
        public virtual int PuntajeEdad { get; set; }
        public virtual int SalarioInicial { get; set; }
        public virtual int SalarioFin { get; set; }
        public virtual string TipoMoneda { get; set; }
        public virtual int PuntajeSalario { get; set; }
        public virtual string IndicadorVerSalario { get; set; }

        public virtual IList<CompetenciaCargo> Competencias { get; set; }
        
        public virtual string ObjetivoCargo { get; set; }
        public virtual string FuncionCargo { get; set; }
        public virtual string ObservacionCargo { get; set; }

        public virtual int PuntajeTotalPostulanteInterno { get; set; }
        public virtual int PuntajeMinimoPostulanteInterno { get; set; }
        public virtual int PuntajeTotalEdad { get; set; }
        public virtual int PuntajeMinimoEdad { get; set; }
        public virtual int PuntajeTotalSexo { get; set; }
        public virtual int PuntajeMinimoSexo { get; set; }
        public virtual int PuntajeTotalSalario { get; set; }
        public virtual int PuntajeMinimoSalario { get; set; }
        public virtual int PuntajeTotalNivelEstudio { get; set; }
        public virtual int PuntajeMinimoNivelEstudio { get; set; }
        public virtual int PuntajeTotalCentroEstudio { get; set; }
        public virtual int PuntajeMinimoCentroEstudio { get; set; }
        public virtual int PuntajeTotalExperiencia { get; set; }
        public virtual int PuntajeMinimoExperiencia { get; set; }
        public virtual int PuntajeTotalFuncionesDesempeñandas { get; set; }
        public virtual int PuntajeMinimoFuncionesDesempeñandas { get; set; }

        public virtual int PuntajeTotalConocimientoGeneral { get; set; }
        public virtual int PuntajeMinimoConocimientoGeneral { get; set; }
        public virtual int PuntajeTotalConocimientoIdioma { get; set; }
        public virtual int PuntajeMinimoConocimientoIdioma { get; set; }
        public virtual int PuntajeTotalDiscapacidad { get; set; }
        public virtual int PuntajeMinimoDiscapacidad { get; set; }
        public virtual int PuntajeTotalHorario { get; set; }
        public virtual int PuntajeMinimoHorario { get; set; }
        public virtual int PuntajeTotalUbigeo { get; set; }
        public virtual int PuntajeMinimoUbigeo { get; set; }
        public virtual int PuntajeTotalExamen { get; set; }
        public virtual int PuntajeMinimoExamen { get; set; }
        public virtual int CantidadPreseleccionados { get; set; }
               
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaPublicacion { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaExpiracion { get; set; }

        public virtual string EstadoActivo { get; set; }


        public Cargo()
        {
            
            
        }
            
    }
}