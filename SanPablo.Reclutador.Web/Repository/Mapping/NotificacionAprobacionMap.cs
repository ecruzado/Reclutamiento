namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class NotificacionAprobacionMap : ClassMap<NotificacionDeAprobacion>
    {
        public NotificacionAprobacionMap()
        {
            Id(m => m.CodigoNotificaionDeAprobacion, "IDENOTIFIAPROBACION");
            References(x => x.ConfiguracionAprobacion).Column("IDECONFIGAPROBACION");
            Map(x => x.CodigoProceso, "CODPROCESO");
            Map(x => x.NumeroSuceso, "NUMSUECESO");
            Map(x => x.CodigoCargo, "IDECARGO");
            Table("NOTIFI_APROBACION");
        }
    }
}
