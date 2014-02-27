﻿

namespace SanPablo.Reclutador.Mapping
{

    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;
    
    public class SolReqPersonalMap : ClassMap<SolReqPersonal>
    {
        public SolReqPersonalMap()
        {

            Id(m => m.IdeSolReqPersonal, "IDESOLREQPERSONAL")
               .GeneratedBy
               .Sequence("SOLREQ_PERSONAL_SQ");

            Map(x => x.CodSolReqPersonal, "CODSOLREQPERSONAL");
            Map(x => x.IdeSede, "IDESEDE");
            Map(x => x.TipPuesto, "TIPPUESTO");
            Map(x => x.DesCargo, "DESCARGO");
            Map(x => x.IdeDependencia, "IDEDEPENDENCIA");
            Map(x => x.IdeDepartamento, "IDEDEPARTAMENTO");
            Map(x => x.IdeArea, "IDEAREA");
            Map(x => x.ObjetivoCargo, "OBJETIVOCARGO");
            Map(x => x.PuntPostuinte, "PUNTPOSTUINTE");
            Map(x => x.PuntRefElaboral, "PUNTREFELABORAL");
            Map(x => x.IndMasculino, "INDMASCULINO");
            Map(x => x.IndFemenino, "INDFEMENINO");
            Map(x => x.PuntSexo, "PUNTSEXO");
            Map(x => x.EdadInicio, "EDADINICIO");
            Map(x => x.edadfin, "EDADFIN");
            Map(x => x.PuntEdad, "PUNTEDAD");
            Map(x => x.SalarioInicial, "SALARIOINICIAL");
            Map(x => x.SalarioFin, "SALARIOFIN");
            Map(x => x.TipMoneda, "TIPMONEDA");
            Map(x => x.PuntSalario, "PUNTSALARIO");
            Map(x => x.IndVerSalario, "INDVERSALARIO");
            Map(x => x.IndSexMascu, "INDSEXMASCU");
            Map(x => x.IndSexFemen, "INDSEXFEMEN");
            Map(x => x.Observacion, "OBSERVACION");
            Map(x => x.PuntTotPostuinte, "PUNTTOTPOSTUINTE");
            Map(x => x.PuntMinPostuinte, "PUNTMINPOSTUINTE");
            Map(x => x.PuntTotEdad, "PUNTTOTEDAD");
            Map(x => x.PuntMinEdad, "PUNTMINEDAD");
            Map(x => x.PuntTotSexo, "PUNTTOTSEXO");
            Map(x => x.PuntMinSexo, "PUNTMINSEXO");
            Map(x => x.PuntTotSalario, "PUNTTOTSALARIO");
            Map(x => x.PuntMinSalario, "PUNTMINSALARIO");
            Map(x => x.PuntTotNivelEst, "PUNTTOTNIVELEST");
            Map(x => x.PuntMinNivelEst, "PUNTMINNIVELEST");
            Map(x => x.PuntTotCentroEst, "PUNTTOTCENTROEST");
            Map(x => x.PuntMinCentroEst, "PUNTMINCENTROEST");
            Map(x => x.PuntTotExpLaboral, "PUNTTOTEXPLABORAL");
            Map(x => x.PuntMinExplaboral, "PUNTMINEXPLABORAL");
            Map(x => x.PuntTotFundEse, "PUNTTOTFUNDESE");
            Map(x => x.PuntMinFundEse, "PUNTMINFUNDESE");
            Map(x => x.PuntTotConoGen     , "PUNTTOTCONOGEN");
            Map(x => x.PuntMinConoGen, "PUNTMINCONOGEN");
            Map(x => x.PuntTotConoIdioma, "PUNTTOTCONOIDIOMA");
            Map(x => x.PuntMinConoIdioma, "PUNTMINCONOIDIOMA");
            Map(x => x.PuntTotDisCapa, "PUNTTOTDISCAPA");
            Map(x => x.PuntMinDisCapa, "PUNTMINDISCAPA");
            Map(x => x.PuntTotHorario, "PUNTTOTHORARIO");
            Map(x => x.PuntMinHorario, "PUNTMINHORARIO");
            Map(x => x.PuntTotUbigeo, "PUNTTOTUBIGEO");
            Map(x => x.PuntMinUbigeo, "PUNTMINUBIGEO");
            Map(x => x.PuntTotReflaboral, "PUNTTOTREFLABORAL");
            Map(x => x.PuntMinReflaboral, "PUNTMINREFLABORAL");
            Map(x => x.PuntTotExamen, "PUNTTOTEXAMEN");
            Map(x => x.PuntMinExamen, "PUNTMINEXAMEN");
            Map(x => x.CantPreSelec, "CANTPRESELEC");
            Map(x => x.FecPublicacion, "FECPUBLICACION");
            Map(x => x.FecExpiracacion, "FECEXPIRACACION");
            Map(x => x.TipVacante, "TIPVACANTE");
            Map(x => x.NumVacantes, "NUMVACANTES");
            Map(x => x.Idecargo, "IDECARGO");
            Map(x => x.NomPersonReemplazo, "NOMPERSONREEMPLAZO");
            Map(x => x.FecIniReemplazo, "FECINIREEMPLAZO");
            Map(x => x.FecfInReemplazo, "FECFINREEMPLAZO");
            Map(x => x.IndCargo, "INDCARGO");
            Map(x => x.IndVerSueldo, "INDVERSUELDO");
            Map(x => x.IndVerSexoMasc, "INDVERSEXOMASC");
            Map(x => x.IndVerSexoFem, "INDVERSEXOFEM");
           
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaModificacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.Sede_des).Formula("(SELECT nvl(s.descripcion,'') descripcion FROM SEDE S WHERE S.IDESEDE=IDESEDE)");
            Map(x => x.Dependencia_des).Formula("(SELECT nvl(d.nomdependencia,'') nomdependencia FROM DEPENDENCIA d WHERE d.IDESEDE=IDESEDE AND d.idedependencia=IDEDEPENDENCIA)");
            Map(x => x.Departamento_des).Formula("select nvl(a.nomdepartamento,'') from departamento a where a.idedepartamento = IDEDEPARTAMENTO and a.idedependencia=IDEDEPENDENCIA)");
            Map(x => x.Area_des).Formula("(select * from area r where r.idearea = IDEAREA and r.idedepartamento=IDEDEPARTAMENTO)");

            Table("SOLREQ_PERSONAL");



        }
    }

}
