namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class ConfiguracionAprobacionMap : ClassMap<ConfiguracionAprobacion>
    {
        public ConfiguracionAprobacionMap()
        {
           Id(m => m.CodigoConfiguracionAprobacion, "IDECONFIGAPROBACION");
            Map(x => x.CodigoProceso, "CODPROCESO");
            Map(x => x.NumeroSuceso, "NUMSUCESO");
            Map(x => x.NumeroSucesoSiguiente, "NUMSUCESOSIG");
            Map(x => x.CodigoCargo, "IDE_CARGO");
            Table("CONFIG_APROBACION");
        }
    }
}