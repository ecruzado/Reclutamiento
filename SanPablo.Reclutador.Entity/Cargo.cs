using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SanPablo.Reclutador.Entity
{
    
    public class Cargo : BaseEntity
    {
        public virtual int IdeCargo { get; set; }
        public virtual int IdeSede { get; set; }
        public virtual string NombreCargo { get; set; }
        public virtual string DescripcionCargo { get; set; }
        public virtual string CodigoCargo { get; set; }
        public virtual int IdeDependencia { get; set; }
        public virtual int IdeDepartamento { get; set; }
        public virtual int IdeArea { get; set; }
        public virtual int? NumeroPosiciones { get; set; }
        public virtual string IndicadorSexo { get; set; }
        public virtual string Sexo { get; set; }
        public virtual int? EdadInicio { get; set; }
        public virtual int? EdadFin { get; set; }
        public virtual string IndicadorEdad { get; set; } 
        public virtual int? PuntajeEdad { get; set; }
        public virtual int? PuntajeSexo { get; set; }
        public virtual string TipoRangoSalarial { get; set; }
        public virtual int? PuntajeSalario { get; set; }
        public virtual string IndicadorSalario { get; set; }
        public virtual string TipoRequerimiento { get; set; }

        public virtual IList<CompetenciaCargo> Competencias { get; set; }
        public virtual IList<OfrecemosCargo> Ofrecimientos { get; set; }
        public virtual IList<HorarioCargo> Horarios { get; set; }
        public virtual IList<UbigeoCargo> Ubigeos { get; set; }
        public virtual IList<CentroEstudioCargo> CentrosEstudios { get; set; }
        public virtual IList<NivelAcademicoCargo> NivelesAcademicos { get; set; }
        public virtual IList<ExperienciaCargo> Experiencias { get; set; }
        public virtual IList<EvaluacionCargo> Evaluaciones { get; set; }
        public virtual IList<ConocimientoGeneralCargo> Conocimientos { get; set; }

        public virtual string ObjetivoCargo { get; set; }
        public virtual string FuncionCargo { get; set; }
        public virtual string ObservacionCargo { get; set; }

        public virtual int? PuntajeTotalPostulanteInterno { get; set; }
        public virtual int? PuntajeTotalEdad { get; set; }
        public virtual int? PuntajeTotalSexo { get; set; }
        public virtual int? PuntajeTotalSalario { get; set; }
       
        public virtual int PuntajeTotalNivelEstudio { get; set; }
        public virtual int PuntajeTotalCentroEstudio { get; set; }
        public virtual int PuntajeTotalExperiencia { get; set; }

        public virtual int PuntajeTotalOfimatica { get; set; }
        public virtual int PuntajeTotalIdioma { get; set; }
        public virtual int PuntajeTotalConocimientoGeneral { get; set; }
        public virtual int PuntajeTotalDiscapacidad { get; set; }
        
        public virtual int PuntajeTotalHorario { get; set; }
        public virtual int PuntajeTotalUbigeo { get; set; }
        public virtual int PuntajeTotalExamen { get; set; }
        public virtual int PuntajeMinimoExamen { get; set; }
        public virtual int CantidadPreseleccionados { get; set; }

        public virtual int PuntajeMinimoGeneral { get; set; }
               
        public virtual string EstadoActivo { get; set; }


        //buscador
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FechaInicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FechaFin { get; set; }

        public virtual bool IndicadorSexoRanking
        {
            get
            {
                return IndicadorSexo == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    IndicadorSexo = Indicador.Si;
                else
                    IndicadorSexo = Indicador.No;
            }
        }

        public virtual bool IndicadorSalarioRanking
        {
            get
            {
                return IndicadorSalario == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    IndicadorSalario = Indicador.Si;
                else
                    IndicadorSalario = Indicador.No;
            }
        }

        public virtual bool IndicadorEdadRanking
        {
            get
            {
                return IndicadorEdad == Indicador.Si ? true : false;
            }
            set
            {
                if (value)
                    IndicadorEdad = Indicador.Si;
                else
                    IndicadorEdad = Indicador.No;
            }
        }
        public Cargo()
        {
            Competencias = new List<CompetenciaCargo>();
            Ofrecimientos = new List<OfrecemosCargo>();
            Horarios = new List<HorarioCargo>();
            Ubigeos = new List<UbigeoCargo>();
            Conocimientos = new List<ConocimientoGeneralCargo>();
            CentrosEstudios = new List<CentroEstudioCargo>();
            NivelesAcademicos = new List<NivelAcademicoCargo>();
            Experiencias = new List<ExperienciaCargo>();
            Evaluaciones = new List<EvaluacionCargo>();
            
        }

        public virtual void agregarCompetencia(CompetenciaCargo competenciaCargo)
        {
            competenciaCargo.Cargo = this;
            Competencias.Add(competenciaCargo);
        }

        public virtual void agregarOfrecimiento(OfrecemosCargo ofrecimientoCargo)
        {
            ofrecimientoCargo.Cargo = this;
            Ofrecimientos.Add(ofrecimientoCargo);
        }

        public virtual void agregarHorarios(HorarioCargo horarioCargo)
        {
            horarioCargo.Cargo = this;
            Horarios.Add(horarioCargo);
        }

        public virtual void agregarUbigeo(UbigeoCargo ubigeoCargo)
        {
            ubigeoCargo.Cargo = this;
            Ubigeos.Add(ubigeoCargo);
        }

        public virtual void agregarConocimiento(ConocimientoGeneralCargo conocimientoCargo)
        {
            conocimientoCargo.Cargo = this;
            Conocimientos.Add(conocimientoCargo);
        }
    }
}