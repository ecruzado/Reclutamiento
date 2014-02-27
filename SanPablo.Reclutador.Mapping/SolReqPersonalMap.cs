

namespace SanPablo.Reclutador.Mapping
{

    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;
    
    public class SolReqPersonalMap : ClassMap<SolReqPersonal>
    {
        public SolReqPersonalMap()
        {

            Id(m => m.IDESOLREQPERSONAL, "IDESOLREQPERSONAL")
               .GeneratedBy
               .Sequence("SOLREQ_PERSONAL_SQ");

            Map(x => x.CODSOLREQPERSONAL, "CODSOLREQPERSONAL");
            Map(x => x.IDESEDE, "IDESEDE");
            Map(x => x.TIPPUESTO, "TIPPUESTO");
            Map(x => x.DESCARGO, "DESCARGO");
            Map(x => x.IDEDEPENDENCIA, "IDEDEPENDENCIA");
            Map(x => x.IDEDEPARTAMENTO, "IDEDEPARTAMENTO");
            Map(x => x.IDEAREA, "IDEAREA");
            Map(x => x.OBJETIVOCARGO, "OBJETIVOCARGO");
            Map(x => x.PUNTPOSTUINTE, "PUNTPOSTUINTE");
            Map(x => x.PUNTREFELABORAL, "PUNTREFELABORAL");
            Map(x => x.INDMASCULINO, "INDMASCULINO");
            Map(x => x.INDFEMENINO, "INDFEMENINO");
            Map(x => x.PUNTSEXO, "PUNTSEXO");
            Map(x => x.EDADINICIO, "EDADINICIO");
            Map(x => x.EDADFIN, "EDADFIN");
            Map(x => x.PUNTEDAD, "PUNTEDAD");
            Map(x => x.SALARIOINICIAL, "SALARIOINICIAL");
            Map(x => x.SALARIOFIN, "SALARIOFIN");
            Map(x => x.TIPMONEDA, "TIPMONEDA");
            Map(x => x.PUNTSALARIO, "PUNTSALARIO");
            Map(x => x.INDVERSALARIO, "INDVERSALARIO");
            Map(x => x.INDSEXMASCU, "INDSEXMASCU");
            Map(x => x.INDSEXFEMEN, "INDSEXFEMEN");
            Map(x => x.OBSERVACION, "OBSERVACION");
            Map(x => x.PUNTTOTPOSTUINTE, "PUNTTOTPOSTUINTE");
            Map(x => x.PUNTMINPOSTUINTE, "PUNTMINPOSTUINTE");
            Map(x => x.PUNTTOTEDAD, "PUNTTOTEDAD");
            Map(x => x.PUNTMINEDAD, "PUNTMINEDAD");
            Map(x => x.PUNTTOTSEXO, "PUNTTOTSEXO");
            Map(x => x.PUNTMINSEXO, "PUNTMINSEXO");
            Map(x => x.PUNTTOTSALARIO, "PUNTTOTSALARIO");
            Map(x => x.PUNTMINSALARIO, "PUNTMINSALARIO");
            Map(x => x.PUNTTOTNIVELEST, "PUNTTOTNIVELEST");
            Map(x => x.PUNTMINNIVELEST, "PUNTMINNIVELEST");
            Map(x => x.PUNTTOTCENTROEST, "PUNTTOTCENTROEST");
            Map(x => x.PUNTMINCENTROEST, "PUNTMINCENTROEST");
            Map(x => x.PUNTTOTEXPLABORAL, "PUNTTOTEXPLABORAL");
            Map(x => x.PUNTMINEXPLABORAL, "PUNTMINEXPLABORAL");
            Map(x => x.PUNTTOTFUNDESE, "PUNTTOTFUNDESE");
            Map(x => x.PUNTMINFUNDESE, "PUNTMINFUNDESE");
            Map(x => x.PUNTTOTCONOGEN     , "PUNTTOTCONOGEN");
            Map(x => x.PUNTMINCONOGEN, "PUNTMINCONOGEN");
            Map(x => x.PUNTTOTCONOIDIOMA, "PUNTTOTCONOIDIOMA");
            Map(x => x.PUNTMINCONOIDIOMA, "PUNTMINCONOIDIOMA");
            Map(x => x.PUNTTOTDISCAPA, "PUNTTOTDISCAPA");
            Map(x => x.PUNTMINDISCAPA, "PUNTMINDISCAPA");
            Map(x => x.PUNTTOTHORARIO, "PUNTTOTHORARIO");
            Map(x => x.PUNTMINHORARIO, "PUNTMINHORARIO");
            Map(x => x.PUNTTOTUBIGEO, "PUNTTOTUBIGEO");
            Map(x => x.PUNTMINUBIGEO, "PUNTMINUBIGEO");
            Map(x => x.PUNTTOTREFLABORAL, "PUNTTOTREFLABORAL");
            Map(x => x.PUNTMINREFLABORAL, "PUNTMINREFLABORAL");
            Map(x => x.PUNTTOTEXAMEN, "PUNTTOTEXAMEN");
            Map(x => x.PUNTMINEXAMEN, "PUNTMINEXAMEN");
            Map(x => x.CANTPRESELEC, "CANTPRESELEC");
            Map(x => x.FECPUBLICACION, "FECPUBLICACION");
            Map(x => x.FECEXPIRACACION, "FECEXPIRACACION");
            Map(x => x.TIPVACANTE, "TIPVACANTE");
            Map(x => x.NUMVACANTES, "NUMVACANTES");
            Map(x => x.IDECARGO, "IDECARGO");
            Map(x => x.NOMPERSONREEMPLAZO, "NOMPERSONREEMPLAZO");
            Map(x => x.FECINIREEMPLAZO, "FECINIREEMPLAZO");
            Map(x => x.FECFINREEMPLAZO, "FECFINREEMPLAZO");
            Map(x => x.INDCARGO, "INDCARGO");
            Map(x => x.INDVERSUELDO, "INDVERSUELDO");
            Map(x => x.INDVERSEXOMASC, "INDVERSEXOMASC");
            Map(x => x.INDVERSEXOFEM, "INDVERSEXOFEM");
            //Map(x => x.USRCREACION, "USRCREACION");
            //Map(x => x.FECCREACION, "FECCREACION");
            //Map(x => x.USRMODIFICA, "USRMODIFICA");
            //Map(x => x.FECMODIFICA, "FECMODIFICA");

            Map(x => x.SEDE_DES).Formula("(SELECT nvl(s.descripcion,'') descripcion FROM SEDE S WHERE S.IDESEDE=IDESEDE)");
            Map(x => x.DEPENDENCIA_DES).Formula("(SELECT nvl(d.nomdependencia,'') nomdependencia FROM DEPENDENCIA d WHERE d.IDESEDE=IDESEDE AND d.idedependencia=IDEDEPENDENCIA)");
            Map(x => x.DEPARTAMENTO_DES).Formula("select nvl(a.nomdepartamento,'') from departamento a where a.idedepartamento = IDEDEPARTAMENTO and a.idedependencia=IDEDEPENDENCIA)");
            Map(x => x.AREA_DES).Formula("(select * from area r where r.idearea = IDEAREA and r.idedepartamento=IDEDEPARTAMENTO)");

            Table("SOLREQ_PERSONAL");

        }
    }

}
