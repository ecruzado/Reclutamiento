

namespace SanPablo.Reclutador.Entity
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    
    public class SolReqPersonal : BaseEntity
    {

        public virtual int IdeSolReqPersonal { get; set; }
        public virtual string CodSolReqPersonal { get; set; }
        public virtual int IdeSede { get; set; }
        public virtual string TipPuesto { get; set; }
        public virtual string DesCargo { get; set; }
        public virtual int IdeDependencia { get; set; }
        public virtual int IdeDepartamento { get; set; }
        public virtual int IdeArea { get; set; }
        public virtual string ObjetivoCargo { get; set; }
        public virtual int PuntPostuinte { get; set; }
        public virtual int PuntRefElaboral { get; set; }
       
        public virtual int PuntSexo { get; set; }
        public virtual int EdadInicio { get; set; }
        public virtual int edadfin { get; set; }
        public virtual int PuntEdad { get; set; }
        public virtual double SalarioInicial { get; set; }
        public virtual double SalarioFin { get; set; }
        public virtual string TipMoneda { get; set; }
        public virtual double PuntSalario { get; set; }
        
        
        public virtual string Observacion { get; set; }
        public virtual string FuncionesCargo { get; set; }
        public virtual string Motivo      { get; set; }
        public virtual int PuntTotPostuinte { get; set; }
        public virtual int PuntMinPostuinte { get; set; }
        public virtual int PuntTotEdad { get; set; }
        public virtual int PuntMinEdad { get; set; }
        public virtual int PuntTotSexo { get; set; }
        public virtual int PuntMinSexo { get; set; }
        public virtual int PuntTotSalario { get; set; }
        public virtual int PuntMinSalario { get; set; }
        public virtual int PuntTotNivelEst { get; set; }
        public virtual int PuntMinNivelEst { get; set; }
        public virtual int PuntTotVentroEst { get; set; }
        public virtual int PuntMinCentroEst { get; set; }
        public virtual int PuntTotExpLaboral { get; set; }
        public virtual int PuntMinExplaboral { get; set; }
        public virtual int PuntTotFundEse { get; set; }
        public virtual int PuntMinFundEse { get; set; }
        public virtual int PuntTotConoGen { get; set; }
        public virtual int PuntMinConoGen { get; set; }
        public virtual int PuntTotConoIdioma { get; set; }
        public virtual int PuntMinConoIdioma { get; set; }
        public virtual int PuntTotDisCapa { get; set; }
        public virtual int PuntMinDisCapa { get; set; }
        public virtual int PuntTotHorario { get; set; }
        public virtual int PuntMinHorario { get; set; }
       
        public virtual int PuntTotUbigeo { get; set; }
        public virtual int PuntMinUbigeo { get; set; }
        public virtual int PuntTotReflaboral { get; set; }
        public virtual int PuntMinReflaboral { get; set; }
        public virtual int PuntTotExamen { get; set; }
        public virtual int PuntMinExamen { get; set; }
        public virtual int CantPreSelec { get; set; }
        public virtual DateTime FecPublicacion { get; set; }
        public virtual DateTime FecExpiracacion { get; set; }
        public virtual string TipVacante { get; set; }
        public virtual int NumVacantes { get; set; }
        public virtual int IdeCargo { get; set; }
        public virtual string NomPersonReemplazo { get; set; }
        public virtual DateTime FecIniReemplazo { get; set; }
        public virtual DateTime FecfInReemplazo { get; set; }
        public virtual string IndCargo { get; set; }
        public virtual string IndVerSueldo { get; set; }
        public virtual string IndVerSexo { get; set; }
        
        public virtual string EstadoActivo { get; set; }
        public virtual string UsrCreacion { get; set; }
        public virtual DateTime Feccreacion { get; set; }
        public virtual string UsrModifica { get; set; }
        public virtual DateTime FecModifica { get; set; }

        public virtual int PuntTotCentroEst { get; set; }

        public virtual string Dependencia_des { get; set; }
        public virtual string Departamento_des { get; set; }
        public virtual string Area_des { get; set; }
        public virtual string Sede_des { get; set; }


        //
        public virtual string IndicadorSexo { get; set; }
        public virtual int PuntajeTotalOfimatica { get; set; }
        public virtual int PuntajeMinimoOfimatica { get; set; }
        public virtual string IndicadorSalario { get; set; }
        public virtual string IndicadorEdad { get; set; }
        public virtual string Sexo { get; set; }
        public virtual string TipoRequerimiento { get; set; }
        public virtual string TipoRangoSalario { get; set; }

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



    }
}
