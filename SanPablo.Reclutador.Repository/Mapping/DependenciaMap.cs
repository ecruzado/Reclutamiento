namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class DependenciaMap : ClassMap<Dependencia>
    {
        public DependenciaMap()
        {
            Id(m => m.CodigoDependencia, "IDEDEPENDENCIA");
            References(x => x.Sede).Column("IDESEDE");
            Map(x => x.NombreDependencia, "NOMBRE");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            Table("DEPENDENCIA");
        }
    }
}