namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class ConfiguracionAprobacionMap : ClassMap<ConfiguracionAprobacion>
    {
        public ConfiguracionAprobacionMap()
        {
            Id(m => m.CodigoConfiguracionAprobacion, "IDECONFIGAPROBACION");
            Map(x => x.CodigoDeProceso, "CODPROCESO");
            Map(x => x.NumeroDeSuceso, "NUMSUCESO");
            Map(x => x.NumeroDeSucesosIG, "NUMSUCESOIG");
            Map(x => x.CodigoCargo, "IDE_CARGO");
            Table("CONFIG_APROBACION");
        }
    }
}