namespace SanPablo.Reclutador.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class LogSolicitudNuevoCargoMap : ClassMap<LogSolicitudNuevoCargo>
    {
        public LogSolicitudNuevoCargoMap()
        {
            Id(m => m.IdeLogSolicitudNuevoCargo, "IDELOGSOLNUEVOCARGO")
                .GeneratedBy
                .Sequence("IDELOGSOLNUEVOCARGO_SQ");
            Map(x => x.IdeSolicitudNuevoCargo, "IDESOLNUEVOCARGO");
            Map(x => x.TipoEtapa, "TIPETAPA");
            Map(x => x.Observacion, "OBSERVACION");
            Map(x => x.RolResponsable, "ROLRESPONSABLE");
            Map(x => x.UsuarioResponsable, "USRESPONSABLE");
            Map(x => x.TipoSuceso, "TIPSUCESO");
            Map(x => x.FechaSuceso, "FECSUCESO");
            Map(x => x.UsuarioSuceso, "USRSUCESO");
            Map(x => x.RolSuceso, "ROLSUCESO");
            Table("LOGSOLNUEVO_CARGO");

        }
    }
}