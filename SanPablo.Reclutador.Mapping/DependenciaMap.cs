namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class DependenciaMap : ClassMap<Dependencia>
    {
        public DependenciaMap()
        {
            Id(m => m.IdeDependencia, "IDEDEPENDENCIA");
            Map(x => x.IdeSede, "IDESEDE");
            //References(x => x.Sede).Column("IDESEDE");
            Map(x => x.NombreDependencia, "NOMDEPENDENCIA");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Table("DEPENDENCIA");
        }
    }
}