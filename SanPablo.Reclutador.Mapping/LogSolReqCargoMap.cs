namespace SanPablo.Reclutador.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class LogSolReqCargoMap : ClassMap<LogSolReqPersonal>
    {
        public LogSolReqCargoMap()
        {
            Id(m => m.IdeLogSolReqPersonal, "IDELOGSOLREQ_PERSONAL")
                .GeneratedBy
                .Sequence("IDELOGSOLREQ_PERSONAL_SQ");
            Map(x => x.IdeSolReqPersonal, "IDESOLREQPERSONAL");
            Map(x => x.TipEtapa, "TIPETAPA");
            Map(x => x.Observacion, "OBSERVACION");
            Map(x => x.RolResponsable, "ROLRESPONSABLE");
            Map(x => x.UsResponsable, "USRESPONSABLE");
            Map(x => x.FecSuceso, "FECSUCESO");
            Map(x => x.UsrSuceso, "USRSUCESO");
            Map(x => x.RolSuceso, "ROLSUCESO");
            Table("LOGSOLREQ_PERSONAL");

        }
    }
}