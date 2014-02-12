namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class AreaMap : ClassMap<Area>
    {
        public AreaMap()
        {
            Id(m => m.IdeArea, "IDEAREA");
            References(x => x.Departamento).Column("IDEDEPARTAMENTO");
            Map(x => x.NombreArea, "NOMAREA");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Table("AREA");
        }
    }
}