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
        public virtual int IdeArea { get; set; }
        public virtual int NumeroPosiciones { get; set; }
        public virtual int PuntajePostulanteInterno { get; set; }
        public virtual string IndicadorSexo { get; set; }
        public virtual string Sexo { get; set; }
        public virtual int EdadInicio { get; set; }
        public virtual int EdadFin { get; set; }
        public virtual string IndicadorEdad { get; set; } 
        public virtual int PuntajeEdad { get; set; }
        public virtual int PuntajeSexo { get; set; }
        public virtual string TipoRangoSalarial { get; set; }
        public virtual string TipoMoneda { get; set; }
        public virtual int PuntajeSalario { get; set; }
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

        public virtual int PuntajeTotalOfimatica { get; set; }
        public virtual int PuntajeMinimoOfimatica { get; set; }
        public virtual int PuntajeTotalIdioma { get; set; }
        public virtual int PuntajeMinimoIdioma { get; set; }
        public virtual int PuntajeTotalConocimientoGeneral { get; set; }
        public virtual int PuntajeMinimoConocimientoGeneral { get; set; }
        public virtual int PuntajeTotalDiscapacidad { get; set; }
        public virtual int PuntajeMinimoDiscapacidad { get; set; }
        
        public virtual int PuntajeTotalHorario { get; set; }
        public virtual int PuntajeMinimoHorario { get; set; }
        public virtual int PuntajeTotalUbigeo { get; set; }
        public virtual int PuntajeMinimoUbigeo { get; set; }
        public virtual int PuntajeTotalExamen { get; set; }
        public virtual int PuntajeMinimoExamen { get; set; }
        public virtual int CantidadPreseleccionados { get; set; }
               
        public virtual string EstadoActivo { get; set; }

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